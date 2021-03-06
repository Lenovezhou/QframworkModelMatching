﻿using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Tool:Singleton<Tool>
{

    #region EventEnums

    /// <summary>
    /// 事件类型
    /// </summary>
    public enum GameEvent { E_Camera, E_MainProcess,E_Mouse,E_Web,E_Loadmodel}


    /// <summary>
    /// 游戏主过程
    /// </summary>
    public enum MainProcess_E { MainProcess_RePlay, MainProcess_SaveALL, MainProcess_loadedUserPointDone, MainProcess_LoadUserDataDone }


    #endregion

    #region 项目枚举
    //使用材质
    public enum MatarialsUse { NormalPoint_hight, NormalPoint_origin, UserImportpoint_hight, UserImportpoint_origin, UserimportModel, NormalModel, Indicator_Effect, Indicator_origin }

    //点的类型
    public enum PointMode { Normal, UserImport }


    #endregion

    #region 全局声明

    static public string normaltag = "Finish";
    static public string userimporttag = "Player";

    static public string LocalJsonSavePath = UnityEngine.Application.dataPath + "/LocalSave/IllNessDataJson/";
    static public string LocalModelonSavePath = UnityEngine.Application.dataPath + "/LocalSave/Model/";
    static public string LocalModelPointsPath = UnityEngine.Application.dataPath + "/ModelPoints/";

    //花纹本地存储路径Resources.load
    static public string LocalDecorativepatternPath = "Decorativepattern";
    //护具的本地存储路径Resources.load
    static public string R_ProtectiveclothingPath = "Model/RightProtectiveclothing";
    static public string L_ProtectiveclothingPath = "Model/LeftProtectiveclothing";
    //坐标系本地位置Resources.load
    static public string CoordinateSystemLocalPath = "Model/CoordinateSystem";
    //标准点到用户点的指示位置Resources.load
    static public string InstructionsGizmoLocalPath = "Model/InstructionsGizmo";
    //服务器下载的模型路径
    static public string SaveDownLoadFromWebPath = UnityEngine.Application.dataPath + "/LocalSave/Download/";
    //本地标准模型存储路径
    static public string LocalNormalModelPath = UnityEngine.Application.dataPath + "/LocalSave/NormalModels/";

    //本地保存的标准件的点json
    static public string LocalNormalpointsJsonPath = UnityEngine.Application.dataPath + "/LocalSave/ModelPoints/NormalModelJson/";

    static public string ModleDefaultPath = "D:/Test/000.obj";

    //格式化消息分割符
    static public string FormatMessageStr = "*";
    static public char UnFormatMessageChar = '*';

    //读取json的key
    static public string datapointkey = "points";
    static public string pointmatchingpoints = "matching_point";
    //下载模型时片体key
    static public string downloadbodykey = "a";
    //下载模型时护具key
    static public string Protectiveclothingkey = "b";

    //受伤部位
    static public List<string> InjuryPosition = new List<string>() { "手", "臂", "脚", "膝" };
    //受伤方向
    static public List<string> directionill = new List<string>() { "左", "右" };
    //护具外形
    static public List<string> protector_shape = new List<string>() { "短护具", "长护具 " };


    //Notice弹出后提示文字
    static public string ConnectingStr = "服务器连接中...";
    static public string FaleToConnect = "服务器连接失败,请联系管理员";
    static public string DownloadDir = "下载中,进度:";


    static public string FaleToSave = "保存失败,原因: ";

    static public int STLViewLayer = LayerMask.NameToLayer("STLView");

    static public Rect ThiredpanelNormalcam = new Rect(0, 0, 1, 1);
    static public Rect ThirdpanelSmallercam = new Rect(-0.17f, 0, 1, 1);

    //规定屏幕初始宽高
    static public Vector2 ScreenSize = new Vector2(1280, 768);
    //默认导入模型的坐标
    static public Vector3 ImprotUserPos = new Vector3(-0.0583f, 0.12736f, 4.03415f);

    //导入模型的缩放比例
    static public float UserImportScaler = 0.008f;
    //初始坐标系位置
    static public Vector3 coordinatesystem_originPos = new Vector3(0, 0, 4);

    /// <summary>
    /// 默认md5
    /// </summary>
    static public string DefultMd5 = "fd44b5075d121e7f347684259aeb15af";


    /// <summary>
    /// stl文件后缀
    /// </summary>
    static public string STLfiledir = ".stl";

    //URL
    static public string URLdir = "https://bio3d.elacg.net/";
    /// <summary>
    /// 登陆时拉取所有病例地址
    /// </summary>
    static public string illnessdatasimplepath = URLdir + "api/cases";
    /// <summary>
    /// 添加病例地址
    /// </summary>
    static public string addillnessdatasimplepath = URLdir + "api/cases/store";
    /// <summary>
    /// 第一界面刷新简易病例地址前缀（后加ID）
    /// </summary>
    static public string refreshillnessdatasimplepath = URLdir + "api/cases/update/";
    /// <summary>
    /// 请求详细病例地址前缀（后加ID）
    /// </summary>
    static public string requestdetailillnessdatapath = URLdir + "api/cases/show/";
    /// <summary>
    /// 添加matchingpoint地址
    /// </summary>
    static public string addmatchingpointspath = URLdir + "api/points/store";
    /// <summary>
    /// 刷新matchingpoints地址（后加ID）
    /// </summary>
    static public string refreshmathcingpointpath = URLdir + "api/points/update/";
    #endregion

    #region File(stl)文件操作
    /// <summary>
    /// 赋值文件，并重命名
    /// </summary>
    /// <param name="srcPath"></param>
    /// <param name="id"></param>
    static public string CopyFileAndRename(string srcPath, int id)
    {
        string tarPath = LocalModelonSavePath;
        string fTarPath = tarPath + "\\" + srcPath.Substring(Path.GetDirectoryName(srcPath).Length + 1);

        CopyFile(srcPath, fTarPath);

        string enddir = ".stl";
        if (fTarPath.IsObj())
        {
            enddir = ".obj";
        }

        File.Move(fTarPath, Path.GetDirectoryName(fTarPath) + "/" + id.ToString() + enddir);
        return Path.GetDirectoryName(fTarPath) + "/" + id.ToString() + enddir;
    }



    /// <summary>
    /// 赋值文件到指定文件夹
    /// </summary>
    static public void CopyFile(string sourcepath, string destpath)
    {
        if (File.Exists(destpath))
        {
            File.Copy(sourcepath, destpath, true);
        }
        else
        {
            File.Copy(sourcepath, destpath);
        }
    }


    /// <summary>
    /// 判断是否存在该路径
    /// </summary>
    /// <param name="newpath"></param>
    /// <returns></returns>
    static public bool CheckFileExist(string newpath)
    {
        return File.Exists(newpath);
    }


    #endregion

    #region 打开windows文件夹
    /// <summary>
    /// 打开选择文件的窗口
    /// </summary>
    /// <returns></returns>
    static public string OpenFileDisplay()
    {
        string path = "";
        OpenFileName ofn = new OpenFileName();

        ofn.structSize = Marshal.SizeOf(ofn);

        ofn.filter = "All Files\0*.*\0\0";

        ofn.file = new string(new char[256]);

        ofn.maxFile = ofn.file.Length;

        ofn.fileTitle = new string(new char[64]);

        ofn.maxFileTitle = ofn.fileTitle.Length;

        ofn.initialDir = UnityEngine.Application.dataPath;//默认路径  

        ofn.title = "Open Project";

        ofn.defExt = "JPG";//显示文件的类型  
                           //注意 一下项目不一定要全选 但是0x00000008项不要缺少  
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR  

        if (DllTest.GetOpenFileName(ofn))
        {
            path = ofn.file;
        }
        return path;

    }

    #endregion

    #region 本地修改模型的transform写入本地
    public static void UpdateLocaluserdataFiles(int id, string data)
    {
        string message = data;
        string FileName = LocalJsonSavePath + id + ".json";

        if (!File.Exists(FileName))
        {
            File.Delete(FileName);
        }

        using (FileStream fileStream = File.Create(FileName))
        {
            byte[] bytes = new UTF8Encoding(true).GetBytes(message);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Dispose();
            fileStream.Close();
        }
    }


    /// <summary>
    /// 读取本地json
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    static public string ReadLocalJson(string filepath)
    {
        string Localjson = "";
        if (File.Exists(filepath))
        {
            StreamReader sr = new StreamReader(filepath);

            Localjson = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
        }
        return Localjson;
    }
    #endregion

    #region 读取文件MD5
    static public string ReadMD5(string filepath)
    {
        try
        {
            FileStream file = new FileStream(filepath, System.IO.FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            Debug.Log("<color=yellow>md5码： " + sb.ToString() + "</color>");
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
        }
    }
    #endregion

    #region 判断是否为该物体的子物体

    public static bool ContainsChild(GameObject parent, string childName)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            if (childName == children[i].name)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region 数字转abc


    static public string NumberToChar(int number)
    {
        if (1 <= number && 36 >= number)
        {
            int num = number + 64;
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            byte[] btNumber = new byte[] { (byte)num };
            return asciiEncoding.GetString(btNumber);
        }
        return "数字不在转换范围内";
    }
    #endregion

    #region abc 转数字
    static public int CharToNumber(string str)
    {
        //定义一组数组array
        byte[] array = new byte[1];
        array = System.Text.Encoding.ASCII.GetBytes(str);
        int asciicode = (short)(array[0]);
        return asciicode;
    }


    #endregion

}






/// <summary>
/// 扩展
/// </summary>
public static class Extension
{
    #region stringextension
    public static bool IsStl(this string path)
    {
        string extension = Path.GetExtension(path);

        if (extension == ".stl" || extension == ".STL")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsObj(this string path)
    {
        string extension = Path.GetExtension(path);

        if (extension == ".obj" || extension == ".OBJ")
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public static bool IsOn(this string value)
    {
        return value == "On";
    }

    #endregion


    #region veticers

    public static int AddGetIndex(this List<Vector3> vertices, Vector3 vec)
    {
        vertices.Add(vec);
        return vertices.Count - 1;
    }

    public static void AddNormal(this List<Vector3> normals, Vector3 vec)
    {
        normals.Add(vec);
        normals.Add(vec);
        normals.Add(vec);
    }

    public static void AddTriangle(this List<int> triangles, int vertex1, int vertex2, int vertex3)
    {
        triangles.Add(vertex1);
        triangles.Add(vertex2);
        triangles.Add(vertex3);
    }

    #endregion



    #region DropDown

    public static void InitDropDown(this Dropdown Dd, List<string> showNames)
    {
        Dd.options.Clear();
        Dropdown.OptionData temoData;
        for (int i = 0; i < showNames.Count; i++)
        {
            //给每一个option选项赋值
            temoData = new Dropdown.OptionData();
            temoData.text = showNames[i];
            // temoData.image = sprite_list[i];
            Dd.options.Add(temoData);
        }
        //初始选项的显示
        Dd.captionText.text = showNames[0];

    }

    #endregion

}





[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}

public class DllTest
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool GetOpenFileName1([In, Out] OpenFileName ofn)
    {
        return GetOpenFileName(ofn);
    }
}

