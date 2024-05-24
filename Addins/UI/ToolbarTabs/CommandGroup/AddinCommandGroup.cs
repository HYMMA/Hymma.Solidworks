// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a model for <see cref="CommandGroup"/>
    /// </summary>
    public class AddinCommandGroup : IAddinCommandGroup
    {
        /// <summary>
        /// this constructor is added to be used in the Fluent API. 
        /// </summary>
        public AddinCommandGroup()
        {

        }
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="userId"> 
        /// If you change the definition of an existing CommandGroup (i.e., add or remove toolbar buttons), you must assign a new unique user-defined UserID to that CommandGroup. <br/>
        /// You must perform this action to avoid conflicts with any previously existing CommandGroup and to allow for backward and forward compatibility of the CommandGroups in your application.<br/>
        /// The user ID and the GUID of the CoClass implementing ISwAddin are a unique pair.</param>
        /// <param name="commands"> a list of <see cref="AddinCommand"/> this group presents</param>
        /// <param name="title">To add a menu item for a CommandGroup to an existing SOLIDWORKS menu, specify the name of a parent menu here.<br/>
        /// <example><c>"&amp;Help\\MyApp Title"</c></example></param>
        /// <param name="description">Description of this AddinCommandGroup</param>
        /// <param name="tooltip">Tool-tip of this AddinCommandGroup</param>
        /// <param name="hint">A Hint for this AddinCommandGroup</param>
        /// <param name="icon"><see cref="Bitmap"/> object as icon for this command group inside the command manager</param>
        /// <param name="hasToolbar">does it have tool-bar</param>
        /// <param name="hasMenu">should it be presented in a menu</param>
        public AddinCommandGroup(int userId, IEnumerable<AddinCommand> commands, string title, string description, string tooltip, string hint, Bitmap icon, bool hasToolbar = true, bool hasMenu = true)
        {
            UserId = userId;
            Commands = commands;
            Title = title;
            Description = description;
            ToolTip = tooltip;
            Hint = hint;
            MainIconBitmap = icon;
            HasToolbar = hasToolbar;
            HasMenu = hasMenu;
        }

        /// <summary>
        /// command icons
        /// </summary>
        protected string[] _commandIcons;

        /// <summary>
        /// group icons
        /// </summary>
        protected string[] _groupIcons;

        #region public properties

        /// <inheritdoc/>
        public IEnumerable<AddinCommand> Commands { get; set; }

        ///<inheritdoc/>
        public bool IgnorePrevious { get; set; }
        ///<inheritdoc/>
        public bool IsRegistered { get; set; }
        ///<inheritdoc/>
        public int UserId
        {
            get; set;
        }
        ///<inheritdoc/>
        public string Title { get; set; } = "Title of this AddinCommandGroup";
        ///<inheritdoc/>
        public string Description { get; set; } = "Description of this AddinCommandGroup";
        ///<inheritdoc/>
        public string ToolTip { get; set; } = "Tool-tip of this AddinCommandGroup";
        ///<inheritdoc/>
        public string Hint { get; set; } = "Hint of this AddinCommandGroup";
        ///<inheritdoc/>
        public int Position { get; set; } = 0;
        ///<inheritdoc/>
        public bool HasToolbar { get; set; } = true;
        ///<inheritdoc/>
        public bool HasMenu { get; set; } = true;
        ///<inheritdoc/>
        public Bitmap MainIconBitmap { get; set; }

        ///<inheritdoc/>
        public DirectoryInfo IconsDir { get; set; }

        //a method to register this command group into solidworks
        /// <summary>
        /// returns list of command strips for this command group
        /// </summary>
        public string[] CommandIcons
        {
            get
            {
                if (_commandIcons == null)
                {
                    //get icons
                    var iconBitmaps = Commands.Select(cmd => cmd.IconBitmap).ToArray();

                    //convert all these icons into strips of standard sizes
                    _commandIcons = GetCommandGroupIconStrips(iconBitmaps, "commandsIcons").ToArray();
                }

                CheckIconsExist(_commandIcons);
                return _commandIcons;
            }
        }


        /// <summary>
        /// returns a list of command group icon in standard solidworks sizes
        /// </summary>
        public string[] GroupIcon
        {
            get
            {
                if (_groupIcons == null)
                {
                    //Get main icon in all sizes
                    //NOTE: because main icon is actually one image we will end up just resizing it
                    _groupIcons = GetCommandGroupIconStrips(new[] { MainIconBitmap }, "mainGroupIcon").ToArray();
                }

                CheckIconsExist(_groupIcons);
                return _groupIcons;
            }
        }


        #endregion


        /// <summary>
        /// Combines images into a sprite horizontally
        /// </summary>
        /// <param name="bitmaps">The bitmaps to combine</param>
        /// <param name="stripeHeight">The sprite height</param>
        /// <returns>a strip of all bitmaps in size stipeHeight (h) x (Qty x stripHeight) (w)</returns>
        private static Bitmap GetStripeImage(Bitmap[] bitmaps, int stripeHeight)
        {
            // Read all images into memory
            Bitmap finalImage = null;
            Bitmap[] images = new Bitmap[bitmaps.Length];
            try
            {

                // Get size
                int width = stripeHeight * bitmaps.Length;
                int height = stripeHeight;

                // Create a bitmap to hold the combined image
                finalImage = new Bitmap(width, height);

                // Get a graphics object from the image so we can draw on it
                using (var g = Graphics.FromImage(finalImage))
                {

                    // Set background color
                    g.Clear(Color.Transparent);

                    // Go through each image and draw it on the final image
                    int offset = 0;
                    for (int i = 0; i < bitmaps.Length; i++)
                    {
                        var file = bitmaps[i];
                        // Read this image
                        var bitmap = new Bitmap(file);
                        images[i] = bitmap;

                        // Scale it to the sprite size
                        var scaleFactor = (float)stripeHeight / Math.Max(bitmap.Width, bitmap.Height);


                        // Draw it onto the new image
                        g.DrawImage(bitmap, new Rectangle(offset, 0, (int)(scaleFactor * bitmap.Width), (int)(scaleFactor * bitmap.Height)));

                        // Move offset to next position
                        offset += stripeHeight;

                    };
                }


                // Return the final image
                return finalImage;
            }
            catch (Exception)
            {
                // Cleanup
                finalImage?.Dispose();
                throw;
            }
            finally
            {
                // Cleanup
                for (int i = 0; i < images.Length; i++)
                {
                    images[i]?.Dispose();
                }
            }
        }

        private void CheckIconsExist(string[] fileList)
        {
            foreach (var file in fileList)
            {
                if (!File.Exists(file))
                {
                    throw new FileNotFoundException($"{file} not found");
                }
            }
        }

        /// <summary>
        /// generate 6-off SolidWorks toolbar sprites in 20, 32, 40, 64, 96, 128 pixels
        /// </summary>
        /// <param name="images">bitmap files to combine together</param>
        /// <param name="filenamePrepend">Prepends this word to the new file, it's a suffix</param>
        /// <returns>address to the strip file</returns>
        private string[] GetCommandGroupIconStrips(Bitmap[] images, string filenamePrepend)
        {
            // If we have no images then throw exception
            if (images.Length < 1)
            {
                throw new ArgumentOutOfRangeException(images.ToString());
            }
            // All output sizes
            var possibleSizes = new[] { 20, 32, 40, 64, 96, 128 };

            //variable to hold address to the strips files
            var stripes = new string[possibleSizes.Length];

            // Now create an image from each of the images, for each file size
            for (int i = 0; i < possibleSizes.Length; i++)
            {
                var size = possibleSizes[i];

                // Combine all bitmaps
                var combinedImage = GetStripeImage(images, size);

                using (combinedImage)
                {
                    try
                    {
                        var sb = new StringBuilder(filenamePrepend);
                        sb.Append(size).Append(".png");
                        stripes[i] = Path.Combine(IconsDir.FullName, sb.ToString());
                        combinedImage.Save(stripes[i], ImageFormat.Png);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            };
            return stripes;
        }
    }
}
