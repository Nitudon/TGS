using System;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine.AssetBundles.GraphTool;
using V1=AssetBundleGraph;

namespace UnityEngine.AssetBundles.GraphTool.DataModel.Version2 {
	public class Settings {
		/*
			if true, ignore .meta files inside AssetBundleGraph.
		*/
		public const bool IGNORE_META = true;

		public const string GUI_TEXT_MENU_OPEN = "Window/AssetBundleGraph/Open Graph Editor";
		public const string GUI_TEXT_MENU_BATCHWINDOW_OPEN = "Window/AssetBundleGraph/Open Batch Build Window";
		public const string GUI_TEXT_MENU_BUILD = "Window/AssetBundleGraph/Build Bundles for Current Platform";
		public const string GUI_TEXT_MENU_BATCHBUILD = "Window/AssetBundleGraph/Build Current Graph Selections";
		public const string GUI_TEXT_MENU_GENERATE = "Window/AssetBundleGraph/Create Node Script";
		public const string GUI_TEXT_MENU_GENERATE_MODIFIER = GUI_TEXT_MENU_GENERATE + "/Modifier Script";
		public const string GUI_TEXT_MENU_GENERATE_PREFABBUILDER = GUI_TEXT_MENU_GENERATE + "/PrefabBuilder Script";
		public const string GUI_TEXT_MENU_GENERATE_CUITOOL = "Window/AssetBundleGraph/Create CUI Tool";

		public const string GUI_TEXT_MENU_GENERATE_POSTPROCESS = GUI_TEXT_MENU_GENERATE + "/Postprocess Script";
		public const string GUI_TEXT_MENU_GENERATE_FILTER = GUI_TEXT_MENU_GENERATE + "/Filter Script";
		public const string GUI_TEXT_MENU_GENERATE_NODE = GUI_TEXT_MENU_GENERATE + "/Custom Node Script";
		public const string GUI_TEXT_MENU_DELETE_CACHE = "Window/AssetBundleGraph/Clear Build Cache";
		
		public const string GUI_TEXT_MENU_DELETE_IMPORTSETTING_SETTINGS = "Window/AssetBundleGraph/Clear Saved ImportSettings";

		public const string ASSETNBUNDLEGRAPH_DATA_PATH = "AssetBundleGraph/SettingFiles";
		public const string ASSETBUNDLEGRAPH_DATABASE_NAME = "AssetReferenceDB.asset";
		public const string ASSETBUNDLEGRAPH_BATCHBUILD_CONFIG_NAME = "BatchBuildConfig.asset";

		public const string ASSETS_PATH = "Assets/";
		public const string ASSETBUNDLEGRAPH_PATH = ASSETS_PATH + "AssetBundleGraph/";
		public const string APPLICATIONDATAPATH_CACHE_PATH = ASSETBUNDLEGRAPH_PATH + "Cache/";
		public const string SCRIPT_TEMPLATE_PATH = ASSETBUNDLEGRAPH_PATH + "Editor/ScriptTemplate/";
		public const string SETTING_TEMPLATE_PATH = ASSETBUNDLEGRAPH_PATH + "Editor/SettingTemplate/";
		public const string USERSPACE_PATH = ASSETBUNDLEGRAPH_PATH + "Generated/Editor/";
		public const string CUISPACE_PATH = ASSETBUNDLEGRAPH_PATH + "Generated/CUI/";

		public const string GRAPH_SEARCH_CONDITION = "t:UnityEngine.AssetBundles.GraphTool.DataModel.Version2.ConfigGraph";

		public const string PREFABBUILDER_CACHE_PLACE	= APPLICATIONDATAPATH_CACHE_PATH + "Prefabs";
		public const string BUNDLEBUILDER_CACHE_PLACE	= APPLICATIONDATAPATH_CACHE_PATH + "AssetBundles";

		public const string IMPORTER_SETTINGS_PLACE		= ASSETBUNDLEGRAPH_PATH + "SavedSettings/ImportSettings";

		public const string SETTINGTEMPLATE_FILE_MODEL		= SETTING_TEMPLATE_PATH + "setting.fbx";
		public const string SETTINGTEMPLATE_FILE_AUDIO		= SETTING_TEMPLATE_PATH + "setting.wav";
		public const string SETTINGTEMPLATE_FILE_TEXTURE	= SETTING_TEMPLATE_PATH + "setting.png";
		public const string SETTINGTEMPLATE_FILE_VIDEO		= SETTING_TEMPLATE_PATH + "setting.m4v";

		public const string UNITY_METAFILE_EXTENSION = ".meta";
		public const string DOTSTART_HIDDEN_FILE_HEADSTRING = ".";
		public const string MANIFEST_FOOTER = ".manifest";
		public const char UNITY_FOLDER_SEPARATOR = '/';// Mac/Windows/Linux can use '/' in Unity.

		public const string BASE64_IDENTIFIER = "B64|";

		public const char KEYWORD_WILDCARD = '*';

		public struct BuildAssetBundleOption {
			public readonly BuildAssetBundleOptions option;
			public readonly string description;
			public BuildAssetBundleOption(string desc, BuildAssetBundleOptions opt) {
				option = opt;
				description = desc;
			}
		}

		public static List<BuildAssetBundleOption> BundleOptionSettings = new List<BuildAssetBundleOption> {
			new BuildAssetBundleOption("Uncompressed AssetBundle", BuildAssetBundleOptions.UncompressedAssetBundle),
			new BuildAssetBundleOption("Disable Write TypeTree", BuildAssetBundleOptions.DisableWriteTypeTree),
			new BuildAssetBundleOption("Deterministic AssetBundle", BuildAssetBundleOptions.DeterministicAssetBundle),
			new BuildAssetBundleOption("Force Rebuild AssetBundle", BuildAssetBundleOptions.ForceRebuildAssetBundle),
			new BuildAssetBundleOption("Ignore TypeTree Changes", BuildAssetBundleOptions.IgnoreTypeTreeChanges),
			new BuildAssetBundleOption("Append Hash To AssetBundle Name", BuildAssetBundleOptions.AppendHashToAssetBundleName),
			new BuildAssetBundleOption("ChunkBased Compression", BuildAssetBundleOptions.ChunkBasedCompression),
			new BuildAssetBundleOption("Strict Mode", BuildAssetBundleOptions.StrictMode)
			#if !UNITY_5_5_OR_NEWER
			,
      		// UnityEditor.BuildAssetBundleOptions does no longer have OmitClassVersions available
			new BuildAssetBundleOption("Omit Class Versions", BuildAssetBundleOptions.OmitClassVersions)
			#endif
		};

		public const float WINDOW_SPAN = 20f;

		public const string GROUPING_KEYWORD_DEFAULT = "/Group_*/";
		public const string BUNDLECONFIG_BUNDLENAME_TEMPLATE_DEFAULT = "bundle_*";

		// by default, AssetBundleGraph's node has only 1 InputPoint. and 
		// this is only one definition of it's label.
		public const string DEFAULT_INPUTPOINT_LABEL = "-";
		public const string DEFAULT_OUTPUTPOINT_LABEL = "+";
		public const string BUNDLECONFIG_BUNDLE_OUTPUTPOINT_LABEL = "bundles";
		public const string BUNDLECONFIG_VARIANTNAME_DEFAULT = "";

		public const string DEFAULT_FILTER_KEYWORD = "";
		public const string DEFAULT_FILTER_KEYTYPE = "Any";

		public const string FILTER_KEYWORD_WILDCARD = "*";

		public const string NODE_INPUTPOINT_FIXED_LABEL = "FIXED_INPUTPOINT_ID";

		public class GUI {
			public const string RESOURCE_BASEPATH = "Assets/AssetBundleGraph/Editor/GUI/GraphicResources/";

			public const float NODE_BASE_WIDTH = 120f;
			public const float NODE_BASE_HEIGHT = 40f;
			public const float NODE_WIDTH_MARGIN = 48f;
			public const float NODE_TITLE_HEIGHT_MARGIN = 8f;

			public const float CONNECTION_ARROW_WIDTH = 12f;
			public const float CONNECTION_ARROW_HEIGHT = 15f;

			public const float INPUT_POINT_WIDTH = 21f;
			public const float INPUT_POINT_HEIGHT = 29f;

			public const float OUTPUT_POINT_WIDTH = 10f;
			public const float OUTPUT_POINT_HEIGHT = 23f;

			public const float FILTER_OUTPUT_SPAN = 32f;

			public const float CONNECTION_POINT_MARK_SIZE = 16f;

			public const float CONNECTION_CURVE_LENGTH = 20f;

			public const float TOOLBAR_HEIGHT = 20f;
			public const float TOOLBAR_GRAPHNAMEMENU_WIDTH = 150f;
			public const int TOOLBAR_GRAPHNAMEMENU_CHAR_LENGTH = 20;

			public const string RESOURCE_SKIN					= RESOURCE_BASEPATH + "NodeStyle.guiskin";
			public const string RESOURCE_CONNECTIONPOINT		= RESOURCE_BASEPATH + "ConnectionPoint.png";

			public const string RESOURCE_INPUT_BG				= RESOURCE_BASEPATH + "InputBG.png";
			public const string RESOURCE_OUTPUT_BG				= RESOURCE_BASEPATH + "OutputBG.png";

			public static readonly Color COLOR_ENABLED = new Color(0.43f, 0.65f, 1.0f, 1.0f);
			public static readonly Color COLOR_CONNECTED = new Color(0.9f, 0.9f, 0.9f, 1.0f);
			public static readonly Color COLOR_NOT_CONNECTED = Color.grey;
			public static readonly Color COLOR_CAN_CONNECT = Color.white;//new Color(0.60f, 0.60f, 1.0f, 1.0f);
			public static readonly Color COLOR_CAN_NOT_CONNECT = new Color(0.33f, 0.33f, 0.33f, 1.0f);
		}
	}
}
