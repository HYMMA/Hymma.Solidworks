// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWorks.Interop.sldworks;

namespace UnitTestProject
{
    internal class DummySolidworks : ISldWorks
    {
        public object OpenDoc(string Name, int Type)
        {
            throw new NotImplementedException();
        }

        public ModelDoc IOpenDoc(string Name, int Type)
        {
            throw new NotImplementedException();
        }

        public object ActivateDoc(string Name)
        {
            throw new NotImplementedException();
        }

        public ModelDoc IActivateDoc(string Name)
        {
            throw new NotImplementedException();
        }

        public void SendMsgToUser(string Message)
        {
            throw new NotImplementedException();
        }

        public object Frame()
        {
            throw new NotImplementedException();
        }

        public Frame IFrameObject()
        {
            throw new NotImplementedException();
        }

        public void ExitApp()
        {
            throw new NotImplementedException();
        }

        public void CloseDoc(string Name)
        {
            throw new NotImplementedException();
        }

        public object NewPart()
        {
            throw new NotImplementedException();
        }

        public PartDoc INewPart()
        {
            throw new NotImplementedException();
        }

        public object NewAssembly()
        {
            throw new NotImplementedException();
        }

        public AssemblyDoc INewAssembly()
        {
            throw new NotImplementedException();
        }

        public object NewDrawing(int TemplateToUse)
        {
            throw new NotImplementedException();
        }

        public DrawingDoc INewDrawing(int TemplateToUse)
        {
            throw new NotImplementedException();
        }

        public int DateCode()
        {
            throw new NotImplementedException();
        }

        public string RevisionNumber()
        {
            throw new NotImplementedException();
        }

        public bool LoadFile(string FileName)
        {
            throw new NotImplementedException();
        }

        public bool AddFileOpenItem(string CallbackFcnAndModule, string Description)
        {
            throw new NotImplementedException();
        }

        public bool AddFileSaveAsItem(string CallbackFcnAndModule, string Description, int Type)
        {
            throw new NotImplementedException();
        }

        public void PreSelectDwgTemplateSize(int TemplateToUse, string TemplateName)
        {
            throw new NotImplementedException();
        }

        public void DocumentVisible(bool Visible, int Type)
        {
            throw new NotImplementedException();
        }

        public object DefineAttribute(string Name)
        {
            throw new NotImplementedException();
        }

        public AttributeDef IDefineAttribute(string Name)
        {
            throw new NotImplementedException();
        }

        public void DisplayStatusBar(bool OnOff)
        {
            throw new NotImplementedException();
        }

        public void CreateNewWindow()
        {
            throw new NotImplementedException();
        }

        public void ArrangeIcons()
        {
            throw new NotImplementedException();
        }

        public void ArrangeWindows(int Style)
        {
            throw new NotImplementedException();
        }

        public void QuitDoc(string Name)
        {
            throw new NotImplementedException();
        }

        public object GetModeler()
        {
            throw new NotImplementedException();
        }

        public Modeler IGetModeler()
        {
            throw new NotImplementedException();
        }

        public object GetEnvironment()
        {
            throw new NotImplementedException();
        }

        public SolidWorks.Interop.sldworks.Environment IGetEnvironment()
        {
            throw new NotImplementedException();
        }

        public object NewDrawing2(int TemplateToUse, string TemplateName, int PaperSize, double Width, double Height)
        {
            throw new NotImplementedException();
        }

        public DrawingDoc INewDrawing2(int TemplateToUse, string TemplateName, int PaperSize, double Width, double Height)
        {
            throw new NotImplementedException();
        }

        public bool SetOptions(string Message)
        {
            throw new NotImplementedException();
        }

        public bool PreviewDoc(ref int HWnd, string FullName)
        {
            throw new NotImplementedException();
        }

        public string GetSearchFolders(int FolderType)
        {
            throw new NotImplementedException();
        }

        public bool SetSearchFolders(int FolderType, string Folders)
        {
            throw new NotImplementedException();
        }

        public bool GetUserPreferenceToggle(int UserPreferenceToggle)
        {
            throw new NotImplementedException();
        }

        public void SetUserPreferenceToggle(int UserPreferenceValue, bool OnFlag)
        {
            throw new NotImplementedException();
        }

        public double GetUserPreferenceDoubleValue(int UserPreferenceValue)
        {
            throw new NotImplementedException();
        }

        public bool SetUserPreferenceDoubleValue(int UserPreferenceValue, double Value)
        {
            throw new NotImplementedException();
        }

        public bool LoadFile2(string FileName, string ArgString)
        {
            throw new NotImplementedException();
        }

        public int GetUserPreferenceIntegerValue(int UserPreferenceValue)
        {
            throw new NotImplementedException();
        }

        public bool SetUserPreferenceIntegerValue(int UserPreferenceValue, int Value)
        {
            throw new NotImplementedException();
        }

        public bool RemoveMenuPopupItem(int DocType, int SelectType, string Item, string CallbackFcnAndModule, string CustomNames, int Unused)
        {
            throw new NotImplementedException();
        }

        public bool RemoveMenu(int DocType, string MenuItemString, string CallbackFcnAndModule)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFileOpenItem(string CallbackFcnAndModule, string Description)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFileSaveAsItem(string CallbackFcnAndModule, string Description, int Type)
        {
            throw new NotImplementedException();
        }

        public bool ReplaceReferencedDocument(string ReferencingDocument, string ReferencedDocument, string NewReference)
        {
            throw new NotImplementedException();
        }

        public int AddMenuItem(int DocType, string Menu, int Postion, string CallbackModuleAndFcn)
        {
            throw new NotImplementedException();
        }

        public int AddMenuPopupItem(int DocType, int SelType, string Item, string CallbackFcnAndModule, string CustomNames)
        {
            throw new NotImplementedException();
        }

        public bool RemoveUserMenu(int DocType, int MenuIdIn, string ModuleName)
        {
            throw new NotImplementedException();
        }

        public int AddToolbar(string ModuleName, string Title, int SmallBitmapHandle, int LargeBitmapHandle)
        {
            throw new NotImplementedException();
        }

        public bool AddToolbarCommand(string ModuleName, int ToolbarId, int ToolbarIndex, string CommandString)
        {
            throw new NotImplementedException();
        }

        public bool ShowToolbar(string ModuleName, int ToolbarId)
        {
            throw new NotImplementedException();
        }

        public bool HideToolbar(string ModuleName, int ToolbarId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveToolbar(string Module, int ToolbarId)
        {
            throw new NotImplementedException();
        }

        public bool GetToolbarState(string Module, int ToolbarId, int ToolbarState)
        {
            throw new NotImplementedException();
        }

        public string GetUserPreferenceStringListValue(int UserPreference)
        {
            throw new NotImplementedException();
        }

        public void SetUserPreferenceStringListValue(int UserPreference, string Value)
        {
            throw new NotImplementedException();
        }

        public bool EnableStereoDisplay(bool BEnable)
        {
            throw new NotImplementedException();
        }

        public bool IEnableStereoDisplay(bool BEnable)
        {
            throw new NotImplementedException();
        }

        public object GetDocumentDependencies(string Document, int Traverseflag, int Searchflag)
        {
            throw new NotImplementedException();
        }

        public string IGetDocumentDependencies(string Document, int Traverseflag, int Searchflag)
        {
            throw new NotImplementedException();
        }

        public int GetDocumentDependenciesCount(string Document, int Traverseflag, int Searchflag)
        {
            throw new NotImplementedException();
        }

        public object OpenDocSilent(string FileName, int Type, ref int Errors)
        {
            throw new NotImplementedException();
        }

        public ModelDoc IOpenDocSilent(string FileName, int Type, ref int Errors)
        {
            throw new NotImplementedException();
        }

        public int CallBack(string CallBackFunc, int DefaultRetVal, string CallBackArgs)
        {
            throw new NotImplementedException();
        }

        public int SendMsgToUser2(string Message, int Icon, int Buttons)
        {
            throw new NotImplementedException();
        }

        public EnumDocuments EnumDocuments()
        {
            throw new NotImplementedException();
        }

        public int LoadAddIn(string FileName)
        {
            throw new NotImplementedException();
        }

        public int UnloadAddIn(string FileName)
        {
            throw new NotImplementedException();
        }

        public bool RecordLine(string Text)
        {
            throw new NotImplementedException();
        }

        public object VersionHistory(string FileName)
        {
            throw new NotImplementedException();
        }

        public string IVersionHistory(string FileName)
        {
            throw new NotImplementedException();
        }

        public int IGetVersionHistoryCount(string FileName)
        {
            throw new NotImplementedException();
        }

        public bool AllowFailedFeatureCreation(bool YesNo)
        {
            throw new NotImplementedException();
        }

        public object GetFirstDocument()
        {
            throw new NotImplementedException();
        }

        public string GetCurrentWorkingDirectory()
        {
            throw new NotImplementedException();
        }

        public bool SetCurrentWorkingDirectory(string CurrentWorkingDirectory)
        {
            throw new NotImplementedException();
        }

        public string GetDataFolder(bool BShowErrorMsg)
        {
            throw new NotImplementedException();
        }

        public bool GetSelectionFilter(int SelType)
        {
            throw new NotImplementedException();
        }

        public void SetSelectionFilter(int SelType, bool State)
        {
            throw new NotImplementedException();
        }

        public object ActivateDoc2(string Name, bool Silent, ref int Errors)
        {
            throw new NotImplementedException();
        }

        public ModelDoc IActivateDoc2(string Name, bool Silent, ref int Errors)
        {
            throw new NotImplementedException();
        }

        public bool GetMouseDragMode(int Command)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentLanguage()
        {
            throw new NotImplementedException();
        }

        public ModelDoc IGetFirstDocument()
        {
            throw new NotImplementedException();
        }

        public bool SanityCheck(int SwItemToCheck, ref int P1, ref int P2)
        {
            throw new NotImplementedException();
        }

        public int AddMenu(int DocType, string Menu, int Position)
        {
            throw new NotImplementedException();
        }

        public int CheckpointConvertedDocument(string DocName)
        {
            throw new NotImplementedException();
        }

        public object OpenDoc2(string FileName, int Type, bool ReadOnly, bool ViewOnly, bool Silent, ref int Errors)
        {
            throw new NotImplementedException();
        }

        public ModelDoc IOpenDoc2(string FileName, int Type, bool ReadOnly, bool ViewOnly, bool Silent, ref int Errors)
        {
            throw new NotImplementedException();
        }

        public object GetMassProperties(string FilePathName, string ConfigurationName)
        {
            throw new NotImplementedException();
        }

        public bool IGetMassProperties(string FilePathName, string ConfigurationName, ref double MPropsData)
        {
            throw new NotImplementedException();
        }

        public string GetLocalizedMenuName(int MenuId)
        {
            throw new NotImplementedException();
        }

        public object GetDocumentDependencies2(string Document, bool Traverseflag, bool Searchflag, bool AddReadOnlyInfo)
        {
            throw new NotImplementedException();
        }

        public string IGetDocumentDependencies2(string Document, bool Traverseflag, bool Searchflag, bool AddReadOnlyInfo)
        {
            throw new NotImplementedException();
        }

        public int IGetDocumentDependenciesCount2(string Document, bool Traverseflag, bool Searchflag, bool AddReadOnlyInfo)
        {
            throw new NotImplementedException();
        }

        public object GetSelectionFilters()
        {
            throw new NotImplementedException();
        }

        public void SetSelectionFilters(object SelType, bool State)
        {
            throw new NotImplementedException();
        }

        public bool GetApplySelectionFilter()
        {
            throw new NotImplementedException();
        }

        public void SetApplySelectionFilter(bool State)
        {
            throw new NotImplementedException();
        }

        public object NewDocument(string TemplateName, int PaperSize, double Width, double Height)
        {
            throw new NotImplementedException();
        }

        public ModelDoc INewDocument(string TemplateName, int PaperSize, double Width, double Height)
        {
            throw new NotImplementedException();
        }

        public string GetDocumentTemplate(int Mode, string TemplateName, int PaperSize, double Width, double Height)
        {
            throw new NotImplementedException();
        }

        public int IGetSelectionFiltersCount()
        {
            throw new NotImplementedException();
        }

        public int IGetSelectionFilters()
        {
            throw new NotImplementedException();
        }

        public void ISetSelectionFilters(int Count, ref int SelType, bool State)
        {
            throw new NotImplementedException();
        }

        public string GetCurrSolidWorksRegSubKey()
        {
            throw new NotImplementedException();
        }

        public void SolidWorksExplorer()
        {
            throw new NotImplementedException();
        }

        public string GetUserPreferenceStringValue(int UserPreference)
        {
            throw new NotImplementedException();
        }

        public bool SetUserPreferenceStringValue(int UserPreference, string Value)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentMacroPathName()
        {
            throw new NotImplementedException();
        }

        public object GetOpenDocumentByName(string DocumentName)
        {
            throw new NotImplementedException();
        }

        public ModelDoc IGetOpenDocumentByName(string DocumentName)
        {
            throw new NotImplementedException();
        }

        public void GetCurrentKernelVersions(out string Version1, out string Version2, out string Version3)
        {
            throw new NotImplementedException();
        }

        public string CreatePrunedModelArchive(string PathName, string ZipPathName)
        {
            throw new NotImplementedException();
        }

        public object OpenDoc3(string FileName, int Type, bool ReadOnly, bool ViewOnly, bool RapidDraft, bool Silent, ref int Errors)
        {
            throw new NotImplementedException();
        }

        public ModelDoc IOpenDoc3(string FileName, int Type, bool ReadOnly, bool ViewOnly, bool RapidDraft, bool Silent, ref int Errors)
        {
            throw new NotImplementedException();
        }

        public int AddToolbar2(string ModuleNameIn, string TitleIn, int SmallBitmapHandleIn, int LargeBitmapHandleIn, int MenuPosIn, int DecTemplateTypeIn)
        {
            throw new NotImplementedException();
        }

        public object OpenModelConfiguration(string PathName, string ConfigName)
        {
            throw new NotImplementedException();
        }

        public int GetToolbarDock(string ModuleIn, int ToolbarIDIn)
        {
            throw new NotImplementedException();
        }

        public void SetToolbarDock(string ModuleIn, int ToolbarIDIn, int DocStatePosIn)
        {
            throw new NotImplementedException();
        }

        public object GetMathUtility()
        {
            throw new NotImplementedException();
        }

        public MathUtility IGetMathUtility()
        {
            throw new NotImplementedException();
        }

        public object OpenDoc4(string FileName, int Type, int Options, string Configuration, ref int Errors)
        {
            throw new NotImplementedException();
        }

        public ModelDoc IOpenDoc4(string FileName, int Type, int Options, string Configuration, ref int Errors)
        {
            throw new NotImplementedException();
        }

        public bool IsRapidDraft(string FileName)
        {
            throw new NotImplementedException();
        }

        public object GetTemplateSizes(string FileName)
        {
            throw new NotImplementedException();
        }

        public bool IGetTemplateSizes(string FileName, out int PaperSize, out double Width, out double Height)
        {
            throw new NotImplementedException();
        }

        public object GetColorTable()
        {
            throw new NotImplementedException();
        }

        public ColorTable IGetColorTable()
        {
            throw new NotImplementedException();
        }

        public void SetMissingReferencePathName(string FileName)
        {
            throw new NotImplementedException();
        }

        public object GetUserUnit(int UnitType)
        {
            throw new NotImplementedException();
        }

        public UserUnit IGetUserUnit(int UnitType)
        {
            throw new NotImplementedException();
        }

        public bool SetMouseDragMode(int Command)
        {
            throw new NotImplementedException();
        }

        public void SetPromptFilename(string FileName)
        {
            throw new NotImplementedException();
        }

        public bool SetAddinCallbackInfo(int ModuleHandle, object AddinCallbacks, int Cookie)
        {
            throw new NotImplementedException();
        }

        public bool AddMenuItem2(int DocumentType, int Cookie, string MenuItem, int Position, string MenuCallback, string MenuEnableMethod, string HintString)
        {
            throw new NotImplementedException();
        }

        public int AddToolbar3(int Cookie, string Title, int SmallBitmapResourceID, int LargeBitmapResourceID, int MenuPositionForToolbar, int DocumentType)
        {
            throw new NotImplementedException();
        }

        public bool RemoveToolbar2(int Cookie, int ToolbarId)
        {
            throw new NotImplementedException();
        }

        public bool AddToolbarCommand2(int Cookie, int ToolbarId, int ToolbarIndex, string ButtonCallback, string ButtonEnableMethod, string ToolTip, string HintString)
        {
            throw new NotImplementedException();
        }

        public bool ShowToolbar2(int Cookie, int ToolbarId)
        {
            throw new NotImplementedException();
        }

        public bool HideToolbar2(int Cookie, int ToolbarId)
        {
            throw new NotImplementedException();
        }

        public bool GetToolbarState2(int Cookie, int ToolbarId, int ToolbarState)
        {
            throw new NotImplementedException();
        }

        public int GetToolbarDock2(int Cookie, int ToolbarId)
        {
            throw new NotImplementedException();
        }

        public bool SetToolbarDock2(int Cookie, int ToolbarId, int DockingState)
        {
            throw new NotImplementedException();
        }

        public ModelDoc2 IActivateDoc3(string Name, bool Silent, ref int Errors)
        {
            throw new NotImplementedException();
        }

        public ModelDoc2 IGetFirstDocument2()
        {
            throw new NotImplementedException();
        }

        public ModelDoc2 INewDocument2(string TemplateName, int PaperSize, double Width, double Height)
        {
            throw new NotImplementedException();
        }

        public ModelDoc2 IGetOpenDocumentByName2(string DocumentName)
        {
            throw new NotImplementedException();
        }

        public ModelDoc2 IOpenDoc5(string FileName, int Type, int Options, string Configuration, ref int Errors)
        {
            throw new NotImplementedException();
        }

        public EnumDocuments2 EnumDocuments2()
        {
            throw new NotImplementedException();
        }

        public object CreatePropertyManagerPage(string Title, int Options, object Handler, ref int Errors)
        {
            throw new NotImplementedException();
        }

        public PropertyManagerPage2 ICreatePropertyManagerPage(string Title, int Options, object Handler, ref int Errors)
        {
            throw new NotImplementedException();
        }

        public object GetAddInObject(string Clsid)
        {
            throw new NotImplementedException();
        }

        public int GetProcessID()
        {
            throw new NotImplementedException();
        }

        public ModelDoc2 OpenDoc6(string FileName, int Type, int Options, string Configuration, ref int Errors, ref int Warnings)
        {
            throw new NotImplementedException();
        }

        public bool AddFileOpenItem2(int Cookie, string MethodName, string Description, string Extension)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFileOpenItem2(int Cookie, string MethodName, string Description, string Extension)
        {
            throw new NotImplementedException();
        }

        public bool AddFileSaveAsItem2(int Cookie, string MethodName, string Description, string Extension, int DocumentType)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFileSaveAsItem2(int Cookie, string MethodName, string Description, string Extension, int DocumentType)
        {
            throw new NotImplementedException();
        }

        public bool AddMenuPopupItem2(int DocumentType, int Cookie, int SelectType, string PopupItemName, string MenuCallback, string MenuEnableMethod, string HintString, string CustomNames)
        {
            throw new NotImplementedException();
        }

        public bool RemoveMenuPopupItem2(int DocumentType, int Cookie, int SelectType, string PopupItemName, string MenuCallback, string MenuEnableMethod, string HintString, string CustomNames)
        {
            throw new NotImplementedException();
        }

        public object GetMassProperties2(string FilePathName, string ConfigurationName, int Accuracy)
        {
            throw new NotImplementedException();
        }

        public bool IGetMassProperties2(string FilePathName, string ConfigurationName, ref double MPropsData, int Accuracy)
        {
            throw new NotImplementedException();
        }

        public void HighlightTBButton(int CmdID)
        {
            throw new NotImplementedException();
        }

        public bool RunMacro(string FilePathName, string ModuleName, string ProcedureName)
        {
            throw new NotImplementedException();
        }

        public int GetConfigurationCount(string FilePathName)
        {
            throw new NotImplementedException();
        }

        public object GetConfigurationNames(string FilePathName)
        {
            throw new NotImplementedException();
        }

        public string IGetConfigurationNames(string FilePathName, int Count)
        {
            throw new NotImplementedException();
        }

        public object GetPreviewBitmap(string FilePathName, string ConfigName)
        {
            throw new NotImplementedException();
        }

        public string GetExecutablePath()
        {
            throw new NotImplementedException();
        }

        public int GetEdition()
        {
            throw new NotImplementedException();
        }

        public int MoveDocument(string SourceDoc, string DestDoc, object FromChildren, object ToChildren, int Option)
        {
            throw new NotImplementedException();
        }

        public int CopyDocument(string SourceDoc, string DestDoc, object FromChildren, object ToChildren, int Option)
        {
            throw new NotImplementedException();
        }

        public int IMoveDocument(string SourceDoc, string DestDoc, int ChildCount, ref string FromChildren, ref string ToChildren, int Option)
        {
            throw new NotImplementedException();
        }

        public int ICopyDocument(string SourceDoc, string DestDoc, int ChildCount, ref string FromChildren, ref string ToChildren, int Option)
        {
            throw new NotImplementedException();
        }

        public int AddToolbar4(int Cookie, string Title, string SmallBitmapImage, string LargeBitmapImage, int MenuPositionForToolbar, int DocumentType)
        {
            throw new NotImplementedException();
        }

        public string GetActiveConfigurationName(string FilePathName)
        {
            throw new NotImplementedException();
        }

        public object Command(int Command, object Args)
        {
            throw new NotImplementedException();
        }

        public object GetRecentFiles()
        {
            throw new NotImplementedException();
        }

        public void ShowBubbleTooltip(int PointAt, string FlashButtonIDs, int TitleResID, string TitleString, string MessageString)
        {
            throw new NotImplementedException();
        }

        public string GetMaterialSchemaPathName()
        {
            throw new NotImplementedException();
        }

        public object GetMaterialDatabases()
        {
            throw new NotImplementedException();
        }

        public int GetMaterialDatabaseCount()
        {
            throw new NotImplementedException();
        }

        public string IGetMaterialDatabases(int Count)
        {
            throw new NotImplementedException();
        }

        public TaskpaneView CreateTaskpaneView(ref int Bitmap, string ToolTip, object PHandler)
        {
            throw new NotImplementedException();
        }

        public void ShowBubbleTooltipAt(int PointX, int PointY, int ArrowPos, string TitleString, string MessageString, string UrlLoc)
        {
            throw new NotImplementedException();
        }

        public void InstallQuickTipGuide(object PInterface)
        {
            throw new NotImplementedException();
        }

        public void UnInstallQuickTipGuide(object PInterface)
        {
            throw new NotImplementedException();
        }

        public void RefreshQuickTipWindow()
        {
            throw new NotImplementedException();
        }

        public int GetLastToolbarID()
        {
            throw new NotImplementedException();
        }

        public PtnrPMPage CreatePMPage(int DialogId, string Title, object Handler)
        {
            throw new NotImplementedException();
        }

        public int GetUserTypeLibReferenceCount()
        {
            throw new NotImplementedException();
        }

        public string IGetUserTypeLibReferences(int NCount)
        {
            throw new NotImplementedException();
        }

        public void ISetUserTypeLibReferences(int NCount, ref string BstrTlbRef)
        {
            throw new NotImplementedException();
        }

        public bool RemoveUserTypeLibReferences(object VTlbRef)
        {
            throw new NotImplementedException();
        }

        public bool IRemoveUserTypeLibReferences(int NCount, ref string BstrTlbRef)
        {
            throw new NotImplementedException();
        }

        public string GetOpenFileName(string DialogTitle, string InitialFileName, string FileFilter, out int OpenOptions, out string ConfigName, out string DisplayName)
        {
            throw new NotImplementedException();
        }

        public void ShowTooltip(string ToolbarName, int ButtonID, int SelectIDMask1, int SelectIDMask2, string TitleString, string MessageString)
        {
            throw new NotImplementedException();
        }

        public bool AddMenuItem3(int DocumentType, int Cookie, string MenuItem, int Position, string MenuCallback, string MenuEnableMethod, string HintString, string BitmapFilePath)
        {
            throw new NotImplementedException();
        }

        public int GetLatestSupportedFileVersion()
        {
            throw new NotImplementedException();
        }

        public void GetOpenedFileInfo(out string FileName, out int Options)
        {
            throw new NotImplementedException();
        }

        public ModelDoc2 GetOpenDocument(string DocName)
        {
            throw new NotImplementedException();
        }

        public object GetImportFileData(string FileName)
        {
            throw new NotImplementedException();
        }

        public bool LoadFile3(string FileName, string ArgString, object ImportData)
        {
            throw new NotImplementedException();
        }

        public TaskpaneView CreateTaskpaneView2(string Bitmap, string ToolTip)
        {
            throw new NotImplementedException();
        }

        public CommandManager GetCommandManager(int Cookie)
        {
            throw new NotImplementedException();
        }

        public void DragToolbarButton(int SourceToolbar, int TargetToolbar, int SourceIndex, int TargetIndex)
        {
            throw new NotImplementedException();
        }

        public void AddCallback(int Cookie, string CallbackFunction)
        {
            throw new NotImplementedException();
        }

        public void RemoveCallback(int Cookie)
        {
            throw new NotImplementedException();
        }

        public void ShowHelp(string HelpFile, int HelpTopic)
        {
            throw new NotImplementedException();
        }

        public int GetErrorMessages(out object Msgs, out object MsgIDs, out object MsgTypes)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentMacroPathFolder()
        {
            throw new NotImplementedException();
        }

        public ModelDoc2 LoadFile4(string FileName, string ArgString, object ImportData, ref int Errors)
        {
            throw new NotImplementedException();
        }

        public bool CloseAllDocuments(bool IncludeUnsaved)
        {
            throw new NotImplementedException();
        }

        public int GetCommandID(string Clsid, int UserCmdID)
        {
            throw new NotImplementedException();
        }

        public bool PreviewDocx64(ref long HWnd, string FullName)
        {
            throw new NotImplementedException();
        }

        public bool GetUserProgressBar(out UserProgressBar PProgressBar)
        {
            throw new NotImplementedException();
        }

        public bool AddFileOpenItem3(int Cookie, string MethodName, string Description, string Extension, string OptionLabel, string OptionMethodName)
        {
            throw new NotImplementedException();
        }

        public int GetCookie(string AddinClsid, int ResourceModuleHandle, object AddinCallbacks)
        {
            throw new NotImplementedException();
        }

        public bool ActivateTaskPane(int TaskPaneID)
        {
            throw new NotImplementedException();
        }

        public object GetExportFileData(int FileType)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFromMenu(int CommandID, int DocumentType, int Option, bool RemoveParentMenu)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFromPopupMenu(int CommandID, int DocumentType, int SelectionType, bool RemoveParentMenu)
        {
            throw new NotImplementedException();
        }

        public string GetMenuStrings(int CommandID, int DocumentType, out string ParentMenuName)
        {
            throw new NotImplementedException();
        }

        public void RefreshTaskpaneContent()
        {
            throw new NotImplementedException();
        }

        public bool PresetNewDrawingParameters(string DrawingTemplate, bool ShowTemplate, double Width, double Height)
        {
            throw new NotImplementedException();
        }

        public void ResetPresetDrawingParameters()
        {
            throw new NotImplementedException();
        }

        public bool GetDocumentVisible(int Type)
        {
            throw new NotImplementedException();
        }

        public bool RunCommand(int CommandID, string NewTitle)
        {
            throw new NotImplementedException();
        }

        public void HideBubbleTooltip()
        {
            throw new NotImplementedException();
        }

        public object GetOpenDocSpec(string FileName)
        {
            throw new NotImplementedException();
        }

        public ModelDoc2 OpenDoc7(object Specification)
        {
            throw new NotImplementedException();
        }

        public bool BlockSkinning()
        {
            throw new NotImplementedException();
        }

        public bool ResumeSkinning()
        {
            throw new NotImplementedException();
        }

        public void SetMultipleFilenamesPrompt(object FileName)
        {
            throw new NotImplementedException();
        }

        public bool GetPreviewBitmapFile(string DocumentPath, string ConfigName, string BitMapFile)
        {
            throw new NotImplementedException();
        }

        public int DragToolbarButtonFromCommandID(int CommandID, int TargetToolbar, int TargetIndex)
        {
            throw new NotImplementedException();
        }

        public int AddMenuItem4(int DocumentType, int Cookie, string MenuItem, int Position, string MenuCallback, string MenuEnableMethod, string HintString, string BitmapFilePath)
        {
            throw new NotImplementedException();
        }

        public int AddMenuPopupItem3(int DocumentType, int Cookie, int SelectType, string PopupItemName, string MenuCallback, string MenuEnableMethod, string HintString, string CustomNames)
        {
            throw new NotImplementedException();
        }

        public void GetBuildNumbers(out string BaseVersion, out string CurrentVersion)
        {
            throw new NotImplementedException();
        }

        public int RegisterTrackingDefinition(string Name)
        {
            throw new NotImplementedException();
        }

        public bool SetNewFilename(string FileName)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentFileUser(string FilePathName)
        {
            throw new NotImplementedException();
        }

        public object GetMacroMethods(string FilePathName, int Filter)
        {
            throw new NotImplementedException();
        }

        public void EnablePhotoWorksProgressiveRender(bool BEnable)
        {
            throw new NotImplementedException();
        }

        public bool RunAttachedMacro(string FileName, string ModuleName, string ProcedureName)
        {
            throw new NotImplementedException();
        }

        public bool RunMacro2(string FilePathName, string ModuleName, string ProcedureName, int Options, out int Error)
        {
            throw new NotImplementedException();
        }

        public bool IsCommandEnabled(int CommandID)
        {
            throw new NotImplementedException();
        }

        public int GetDocumentCount()
        {
            throw new NotImplementedException();
        }

        public object GetDocuments()
        {
            throw new NotImplementedException();
        }

        public ModelDoc2 IGetDocuments(int NumDocuments)
        {
            throw new NotImplementedException();
        }

        public ModelView GetModelView(string ModelName, int WindowID, int Row, int Column)
        {
            throw new NotImplementedException();
        }

        public int ResetUntitledCount(int PartValue, int AssemValue, int DrawingValue)
        {
            throw new NotImplementedException();
        }

        public bool GetToolbarVisibility(int Toolbar)
        {
            throw new NotImplementedException();
        }

        public void SetToolbarVisibility(int Toolbar, bool Visibility)
        {
            throw new NotImplementedException();
        }

        public object GetLastSaveError(out object FilePath, out object ErrorCode)
        {
            throw new NotImplementedException();
        }

        public int RegisterThirdPartyPopupMenu()
        {
            throw new NotImplementedException();
        }

        public bool AddItemToThirdPartyPopupMenu(int RegisterId, int DocType, string Item, string CallbackFcnAndModule, string CustomName, string HintString, string BitmapFileName, int MenuItemTypeOption)
        {
            throw new NotImplementedException();
        }

        public bool ShowThirdPartyPopupMenu(int RegisterId, int Posx, int Posy)
        {
            throw new NotImplementedException();
        }

        public int IsSame(object Object1, object Object2)
        {
            throw new NotImplementedException();
        }

        public bool GetButtonPosition(int PointAt, out int LocX, out int LocY)
        {
            throw new NotImplementedException();
        }

        public bool RunJournalCmd(string Cmd)
        {
            throw new NotImplementedException();
        }

        public bool SetThirdPartyPopupMenuState(int RegisterId, bool IsActive)
        {
            throw new NotImplementedException();
        }

        public bool IsBackgroundProcessingCompleted(string FilePath)
        {
            throw new NotImplementedException();
        }

        public void ShowBubbleTooltipAt2(int PointX, int PointY, int ArrowPos, string TitleString, string MessageString, int TitleBitmapID, string TitleBitmap, string UrlLoc, int Cookie, int LinkStringID, string LinkString, string CallBack)
        {
            throw new NotImplementedException();
        }

        public bool RemoveItemFromThirdPartyPopupMenu(int RegisterId, int DocType, string Item, int IconIndex)
        {
            throw new NotImplementedException();
        }

        public void PostMessageToApplication(int Cookie, int UserData)
        {
            throw new NotImplementedException();
        }

        public int GetCookiex64(string AddinClsid, long ResourceModuleHandle, object AddinCallbacks)
        {
            throw new NotImplementedException();
        }

        public void GetRunningCommandInfo(out int CommandID, out string PMTitle, out bool IsUiActive)
        {
            throw new NotImplementedException();
        }

        public RoutingSettings GetRoutingSettings()
        {
            throw new NotImplementedException();
        }

        public bool GetLineStyles(string StyleFile, out object StyleNameList, out object StyleList)
        {
            throw new NotImplementedException();
        }

        public object GetRayTraceRenderer(int RendererType)
        {
            throw new NotImplementedException();
        }

        public RayTraceRenderer IGetRayTraceRenderer(int RendererType)
        {
            throw new NotImplementedException();
        }

        public bool RecordLineVBnet(string StringLine)
        {
            throw new NotImplementedException();
        }

        public bool RecordLineCSharp(string StringLine)
        {
            throw new NotImplementedException();
        }

        public bool AddItemToThirdPartyPopupMenu2(int RegisterId, int DocType, string Item, int Identifier, string CallbackFunction, string EnableFunction, string CustomName, string HintString, string BitmapFileName, int MenuItemTypeOption)
        {
            throw new NotImplementedException();
        }

        public void PostMessageToApplicationx64(int Cookie, long UserData)
        {
            throw new NotImplementedException();
        }

        public int AddMenuPopupItem4(int DocumentType, int Cookie, string SelectType, string PopupItemName, string MenuCallback, string MenuEnableMethod, string HintString, string CustomNames)
        {
            throw new NotImplementedException();
        }

        public void GetBuildNumbers2(out string BaseVersion, out string CurrentVersion, out string HotFixes)
        {
            throw new NotImplementedException();
        }

        public void SetPromptFilename2(string FileName, string ConfigName)
        {
            throw new NotImplementedException();
        }

        public int CloseAndReopen(ModelDoc2 Doc, int Option, out ModelDoc2 NewDoc)
        {
            throw new NotImplementedException();
        }

        public object ActivateDoc3(string Name, bool UseUserPreferences, int Option, ref int Errors)
        {
            throw new NotImplementedException();
        }

        public bool CopyAppearance(object Object)
        {
            throw new NotImplementedException();
        }

        public bool PasteAppearance(object Object, int AppearanceTarget)
        {
            throw new NotImplementedException();
        }

        public object GetSafeArrayUtility()
        {
            throw new NotImplementedException();
        }

        public bool SetAddinCallbackInfo2(long ModuleHandle, object AddinCallbacks, int Cookie)
        {
            throw new NotImplementedException();
        }

        public bool SanityCheck4(int SwItemToCheck, ref int P1, ref int P2, out long P3)
        {
            throw new NotImplementedException();
        }

        public int AddToolbar5(int Cookie, string Title, object ImageList, int MenuPositionForToolbar, int DocumentType)
        {
            throw new NotImplementedException();
        }

        public int AddMenuItem5(int DocumentType, int Cookie, string MenuItem, int Position, string MenuCallback, string MenuEnableMethod, string HintString, object ImageList)
        {
            throw new NotImplementedException();
        }

        public int GetInterfaceBrightnessThemeColors(out object Colors)
        {
            throw new NotImplementedException();
        }

        public int GetCurrentLicenseType()
        {
            throw new NotImplementedException();
        }

        public int GetImageSize(out int Small, out int Medium, out int Large)
        {
            throw new NotImplementedException();
        }

        public TaskpaneView CreateTaskpaneView3(object ImageList, string ToolTip)
        {
            throw new NotImplementedException();
        }

        public int SaveSettings(string FileName, bool SystemOptions, int ToolbarLayout, bool KeyboardShortcuts, bool MouseGestures, bool MenuCustomization, bool SavedViews)
        {
            throw new NotImplementedException();
        }

        public int RestoreSettings(string FileName, bool SystemOptions, bool ToolbarLayout, bool KeyboardShortcuts, bool MouseGestures, bool MenuCustomization, bool SavedViews, bool CreateBackup)
        {
            throw new NotImplementedException();
        }

        public int ExportHoleWizardItem(string StdToExport, string DestinationFolderPath)
        {
            throw new NotImplementedException();
        }

        public int ImportHoleWizardItem(string StdToImport, string DestinationFilePath, bool ReplaceData, bool ErrorFile)
        {
            throw new NotImplementedException();
        }

        public int ExportToolboxItem(string StdToExport, string DestinationFolderPath)
        {
            throw new NotImplementedException();
        }

        public int ImportToolboxItem(string StdToImport, string DestinationFilePath)
        {
            throw new NotImplementedException();
        }

        public bool SanityCheck6(int SwItemToCheck, ref int P1, ref int P2, out long P3, out int P4, out int P5)
        {
            throw new NotImplementedException();
        }

        public bool SanityCheck5(int SwItemToCheck, ref int P1, ref int P2, out string P3)
        {
            throw new NotImplementedException();
        }

        public string GetSSOFormattedURL(string TargetUrl)
        {
            throw new NotImplementedException();
        }

        public object ActiveDoc { get; }
        public ModelDoc IActiveDoc { get; }
        public bool Visible { get; set; }
        public bool UserControl { get; set; }
        public string ActivePrinter { get; set; }
        public ModelDoc2 IActiveDoc2 { get; }
        public object UserTypeLibReferences { get; set; }
        public bool UserControlBackground { get; set; }
        public bool CommandInProgress { get; set; }
        public bool TaskPaneIsPinned { get; set; }
        public JournalManager JournalManager { get; }
        public int FrameLeft { get; set; }
        public int FrameTop { get; set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public int FrameState { get; set; }
        public bool EnableFileMenu { get; set; }
        public bool EnableBackgroundProcessing { get; set; }
        public bool StartupProcessCompleted { get; }
    }
}
