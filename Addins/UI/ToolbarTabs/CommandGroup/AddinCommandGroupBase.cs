using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using static Hymma.Solidworks.Addins.Logger;

namespace Hymma.Solidworks.Addins
{
    ///<inheritdoc/>
    public abstract class AddinCommandGroupBase : IAddinCommandGroup
    {
        #region protected vars
        /// <summary>
        /// command icons
        /// </summary>
        protected string[] _commandIcons;

        /// <summary>
        /// group icons
        /// </summary>
        protected string[] _groupIcons;
        #endregion

        #region public properties

        /// <inheritdoc/>
        public AddinCommand[] Commands { get; set; }

        ///<inheritdoc/>
        public bool IgnorePrevious { get; set; }
        ///<inheritdoc/>
        public bool IsRegistered { get; protected set; }
        ///<inheritdoc/>
        public int UserId { get; set; }
        ///<inheritdoc/>
        public string Title { get; set; } = "Title of this AddinCommandGroup";
        ///<inheritdoc/>
        public string Description { get; set; } = "Description of this AddinCommandGroup";
        ///<inheritdoc/>
        public string ToolTip { get; set; } = "Tooltip of this AddinCommandGroup";
        ///<inheritdoc/>
        public string Hint { get; set; } = "Hint of this AddinCommandGroup";
        ///<inheritdoc/>
        public int Position { get; set; } = 0;
        ///<inheritdoc/>
        public bool HasToolbar { get; set; } = true;
        ///<inheritdoc/>
        public bool HasMenue { get; set; } = true;
        ///<inheritdoc/>
        public Bitmap MainIconBitmap { get; set; }

        /// <summary>
        /// directory to save the icons
        /// </summary>
        public string IconsDir { get; set; }

        //a method to register this command group into solidworks
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandManager"></param>
        public virtual void Register(ICommandManager commandManager) { }

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
            set { _commandIcons = value; }
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
            set { _groupIcons = value; }
        }


        #endregion

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
                var combinedImage = CombineBitmaps(images, size);

                Log("attempting to save combinedImage");
                using (combinedImage)
                {
                    try
                    {
                        var stripe = $"{filenamePrepend}{size}.png";

                        //make a directory in the default icon filder for this command group only IconsDir is not defined specifically
                        var dir = string.IsNullOrEmpty(IconsDir) ? AddinMaker.GetIconsDir().CreateSubdirectory("grp"+UserId.ToString()).FullName : IconsDir;

                        stripes[i] = Path.Combine(dir, stripe);
                        combinedImage.Save(stripes[i]);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            };
            return stripes;
        }

        /// <summary>
        /// Combines images into a sprite horizontally
        /// </summary>
        /// <param name="bitmaps">The bitmaps to combine</param>
        /// <param name="iconSize">The sprite size</param>
        /// <returns></returns>
        private Bitmap CombineBitmaps(Bitmap[] bitmaps, int iconSize)
        {
            // Read all images into memory
            Bitmap finalImage = null;
            Bitmap[] images = new Bitmap[bitmaps.Length];
            try
            {

                // Get size
                int width = iconSize * bitmaps.Length;
                int height = iconSize;

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
                        var scaleFactor = (float)iconSize / Math.Max(bitmap.Width, bitmap.Height);


                        // Draw it onto the new image
                        g.DrawImage(bitmap, new Rectangle(offset, 0, (int)(scaleFactor * bitmap.Width), (int)(scaleFactor * bitmap.Height)));

                        // Move offset to next position
                        offset += iconSize;

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
                    if (images[i] != null)
                        images[i].Dispose();
                }
            }
        }
    }
}
