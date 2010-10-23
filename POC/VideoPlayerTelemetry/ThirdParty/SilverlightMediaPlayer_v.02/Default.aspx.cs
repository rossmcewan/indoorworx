//******************************************************************************
// Module           :   Default.aspx.cs
// Description      :   code behind
//******************************************************************************
// Author           :   Alexander Bell
// Copyright        :   2010 Infosoft International Inc.
// DateCreated      :   03/17/2010
// LastModified     :   03/18/2010
//******************************************************************************
// DISCLAIMER: This Application is provide on AS IS basis without any warranty
//******************************************************************************

//******************************************************************************
// TERMS OF USE     :   This module is copyrighted.
//                  :   You can use it at your sole risk provided that you keep
//                  :   the original copyright note. 
//NOTE: ALL .WMV/.JPG FILES ARE FOR DEMO PURPOSE ONLY: DO NOT COPY/DISTRIBUTE!
//******************************************************************************
using System;
using Infosoft_SilverlightMediaPlayer;
using System.Collections;

public partial class ASPX_Default : System.Web.UI.Page 
{
    // media source 
    // NOTE: THIS .WMV FILE IS FOR DEMO PURPOSE ONLY: DO NOT COPY/DISTRIBUTE!!!
    //*************************************************************************

    #region page load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            #region Media Player initial settings
                PlayerInit(ref this.MediaPlayer1);
            #endregion

            #region select media item from XML file
                SetMediaItem(ref this.MediaPlayer1);
            #endregion

            #region Dropdown Style selector
            // populate dropdown with Media Player styles (skin): looping through enum
            foreach (MediaPlayerSkins skin in Enum.GetValues(typeof(MediaPlayerSkins)))
            { 
                cmbSkins.Items.Add(skin.ToString()); 
            }

            // set dropdown autopostback
            cmbSkins.AutoPostBack = true;

            // select Professional style
            cmbSkins.SelectedValue = MediaPlayerSkins.Professional.ToString(); 
            
            MediaPlayer1.MediaSkinSource =
            "~/MediaPlayerSkins/" + cmbSkins.SelectedValue + ".xaml";

            #endregion
        }
    }
    #endregion

    #region player init settings from XML file
    private void PlayerInit(ref System.Web.UI.SilverlightControls.MediaPlayer SLP)
    {
        SLMediaPlayer slm;
        try
        {
            // read setting from XML file
            slm = new SLMediaPlayer();
            slm.GetPlayer();

            // assign values to Silverlight Media Player
            SLP.Width = slm.Width;
            SLP.Height = slm.Height;

            SLP.AutoLoad = slm.AutoLoad;
            SLP.AutoPlay = slm.AutoPlay;

            // read from XML and set player scale mode
            switch (slm.ScaleMode)
            {
                case "stretch":
                    SLP.ScaleMode = System.Web.UI.SilverlightControls.ScaleMode.Stretch;
                    break;
                case "zoom":
                    SLP.ScaleMode = System.Web.UI.SilverlightControls.ScaleMode.Zoom;
                    break;
                case "none":
                    SLP.ScaleMode = System.Web.UI.SilverlightControls.ScaleMode.None;
                    break;
                default: break;
            }
        }
        catch { }
        finally { slm = null; }
    }
    #endregion

    #region read media item properties from XML file
    private void SetMediaItem(ref System.Web.UI.SilverlightControls.MediaPlayer SLP)
    {
        MediaItems mi=new MediaItems();
        System.Web.UI.SilverlightControls.MediaChapter mc;
        try
        {
            // get Media item from XML
            mi.GetMediaItem();

            // set Media source
            SLP.MediaSource = mi.MediaSource;
            SLP.PlaceholderSource = mi.PlaceholderSource;

            // add Chapters
            if (mi.Chapters.Count > 0)
            {
                for (int i = 0; i < mi.Chapters.Count; i++)
                {
                    mc = new System.Web.UI.SilverlightControls.MediaChapter();
                    mc.Position = mi.Chapters[i].Position;
                    mc.ThumbnailSource = mi.Chapters[i].ThumbnailSource;
                    mc.Title = mi.Chapters[i].Title;
                    SLP.Chapters.Add(mc);
                    mc = null;
                }
            }
        }
        catch { }
        finally { mi = null; mc = null; }
    }
    #endregion

    #region User Events
    ///<summary>Select Media Player Skin</summary>
    protected void cmbSkins_SelectedIndexChanged(object sender, EventArgs e)
    {
        MediaPlayer1.MediaSkinSource =
            "~/MediaPlayerSkins/" + cmbSkins.SelectedValue + ".xaml";
    }
    #endregion
}
//******************************************************************************