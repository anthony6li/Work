using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JsonTestServer
{
    #region enum        
    public enum JsonRequestType
    {
        hedajwreq,          // "api/hedajwreq"
        hedacmdreq,         // "api/hedacmdreq" 摄像头批量处理
        upload,             //  "xml/upload"
        sysinfo,            //"api/sysinfo"获取系统运行情况
    }

    public enum JsonMethodType
    {
        Version,            //获取系统版本
        RunStatus,          //获取系统运行状态
        SysInfo,            //获取系统运行情况
        UserLogSearch,      //用户日志查询
        ///权限管理
        AddPrivilege,       //添加权限
        UpdatePrivilege,    //修改权限
        DeletePrivilege,    //删除权限
        RolePrivilege,      //获取指定角色的所有权限
        PrivilegeDevice,    //根据权限查询所有设备
        GetPrivilege,       // 获取所有权限
        //角色管理
        AddRole,            //添加角色  
        UpdateRole,         //修改角色  
        DeleteRole,         //删除角色   
        UserRole,           //获取指定用户的所有角色  
        GetRole,            //获取所有角色  
        //下级平台管理
        AddPlat,            //下级平台添加  
        UpdatePlat,         //修改子平台  
        DeletePlat,         //删除平台  
        PlatList,           //获取平台列表  
        PlatListExact,      //条件获取平台列表  
        PlatInfo,           //获取当前平台信息  
        //组管理
        AddGroup,           //添加组
        UpdateGroup,        //修改组
        DeleteGroup,        //删除组
        PlatGroup,          //获取平台下所有组
        GroupSearch,        //组查询
        //云台控制
        KeepMove,           //移动
        StopMove,           //停止移动
        RltMove,            //相对移动
        GetPresets,         //获取预置点
        SetPresets,         //设置预置点
        RemovePresets,      //删除预置点
        GotoPresets,        //移动到预置点
        //视频转发
        RtspRelay,          //RTSP转发
        RtspRelayBat,       //RTSP批量转发
        TSFileRelay,        //录像文件回放
        TSFileSeekPlay,     //回放进度调整
        RmsFtpInfo,         //视频下载配置信息获取
        DownloadFileList,   //DownloadFileList
        GetRelayRtsp,       //获取设备转发地址(内部使用)
        //计划任务
        AddPlan,            //添加计划
        UpdatePlan,         //修改计划
        PlanSearch,         //查询计划
        AllPlanSearch,      //查询所有计划
        ExactPlanSearch,    //精确查询计划
        DeletePlan,         //删除计划
        ClosePlan,          //启动/关闭任务计划
        BatAddPlan,         //批量添加计划任务
        //视频预览
        MP4Preview,         //视频预览
        DevicePreview,      //视频预览(下级平台设备)
        StopPreview,        //停止预览
        //系统配置
        GetRMSConf,         //获取录像服务器(RMS)配置
        RMSConf,            //录像服务器(RMS)配置
        GetDBConf,          //获取数据库配置
        DBConf,             //数据库配置
        CheckDB,            //测试数据库连接
        ReStart,            //重启服务
        //设备管理
        AddDevice,          //添加对接设备
        AddCamera,          //添加摄像头/麦克风
        DelDevice,          //删除对接设备
        DeleteCamera,       //删除摄像头/麦克风
        UpdateDevice,       //修改对接设备
        UpdateCamera,       //修改摄像头/麦克风
        DeviceSearch,       //查询对接设备
        CameraSearch,       //查询摄像头/麦克风
        BatAddDeviceFromIP, //摄像头批量处理-IP段添加
        BatAddDeviceFromXml,//摄像头批量处理-IP段添加
        BatAddDeviceImport, //
        BatPingDevice,      //摄像头批量处理-测试设备是否连接
        SaveDevFile,        //摄像头批量处理-导出设备xml
        AlarmSupplier,      //获取“报警主机”设备供应商
        BindAlarmCamera,    //报警主机和摄像头绑定
        RebindAlarmCamera,  //报警主机和摄像头解绑定
        GetAlarmCamera,     //查询报警主机和摄像头绑定信息
        BASupplier,         //获取“行为分析服务器”设备供应商
        BindBACamera,       //智能分析仪通道与摄像头绑定
        GetBACamera,        //查询智能分析仪通道与摄像头绑定信息
        DeleteBACamera,     //删除智能分析仪通道与摄像头绑定信息
        OpenBACamera,       //启用和关闭智能分析仪通道与摄像头绑定信息
        CVRSupplier,        //获取“中央存储服务器(CVR)”设备供应商
        BindCVRCamera,      //中央存储通道与摄像头绑定
        GetCVRCamera,       //查询中央存储通道与摄像头绑定信息
        DeleteCVRCamera,    //删除中央存储通道与摄像头绑定信息
        GetCVRFileList,     //查询录像文件列表
        StartDownloadFile,  //开始下载录像文件
        GetDownloadPos,     //获取下载进度
        StopDownload,     //停止下载
        StartDownloadAreaFile,     //开始下载房间所有指定时间内录像文件
        PTZControl,     //云台控制
        PTZPreset,     //预置点控制
        DecoderSupplier,     //获取“解码器”设备供应商
        DoorCardSupplier,     //获取“门禁”设备供应商
        GetDoorCardLog,     //查询刷卡记录
        NetKeySupplier,     //获取“网络键盘”设备供应商
        PSSupplier,     //获取“电源时序器”设备供应商
        GetSequencerConf,     //获取电源时序器启动配置信息
        SequencerConf,     //添加电源时序器启动配置信息
        DeleteSequencerConf,     //删除电源时序器启动配置信息
        SyncRecordSupplier,     //获取“同步录音录像”设备供应商
        BindSyncRecordCamera,     //同录设备通道与摄像头绑定
        GetSyncRecordCamera,     //查询同录通道与摄像头绑定信息
        DeleteSyncRecordCamera,     //删除同录通道与摄像头绑定信息
        AddArea,     //添加房间
        DelArea,     //删除房间
        UpdateArea,     //修改房间
        GetArea,     //查询房间
        BindAreaDevice,     //绑定房间和设备
        GetAreaDevice,     //查询房间关联对接设备信息
        GetAreaCamera,     //查询房间摄像头信息
        AddUser,     //添加用户
        UpdateUser,     //修改用户(包括权限)
        UpdateUserInfo,     //修改用户资料(个人中心修改自身信息)
        DeleteUser,     //删除用户
        UserLogin,     //用户登录
        UserSearch,     //用户信息查询
        CheckUserID,     //校验用户ID
        ResetUserPwd,     //重置密码
        GetUser,     //获取所有用户
        UpdatePwd,     //修改用户密码
        BindUserRole,     //绑定用户角色
        //数据同步
        AllDataSync,        //同步所有数据(内部使用)
        DataSync,           //同步部分数据(内部使用)
        ReqDataSync,        //父平台向下级请求同步数据(内部使用)
    }
    #endregion

    #region Struct
    /// <summary>
    /// IP导入摄像头时,指定IP
    /// </summary>
    public struct arrayIP
    {
        public string ip;
    }

    /// <summary>
    /// xml配置导入摄像头时，每个摄像头的信息
    /// </summary>
    public struct arrayXml
    {
        public string devicetype;
        public string devicename;
        public string loginid;
        public string loginpwd;
        public string ip;
        public string port;
        public string mainrtsp;
        public string auxrtsp;
        public string flag;
        public string devicestate;
        public string note;
        public string mic;
        public string radio;
    }

    /// <summary>
    /// 指定设备的ID，Name和Type
    /// </summary>
    public struct arrayArea
    {
        public string deviceid;
        public string devicename;
        public string devicetype;
    }

    /// <summary>
    /// 用户角色ID集合
    /// </summary>
    public struct arrayUser
    {
        public string roleid;
    }

    /// <summary>
    /// 添加权限时的设备集合
    /// </summary>
    public struct arrayPrivilege
    {
        public string deviceid;
        public string devicename;
    }

    /// <summary>
    /// 添加角色时的权限集合
    /// </summary>
    public struct arrayRole
    {
        public string privilegeid;
        public string privilegename;
    }

    public struct arrayPlan
    {
        public string deviceid;
        public string planname;
    }

    public struct arrayDataSync1
    {
        public string bsoid;
        public string createtime;
        public string updatetime;
        public string platformid;
        public string platformname;
        public string parentplatformid;
        public string childplatformid;
        public string ip;
        public string port;
        public string lvl;
    }
    public struct arrayDataSync2
    {
        public string bsoid;
        public string createtime;
        public string updatetime;
        public string groupid;
        public string groupname;
        public string parentgroup;
        public string platformID;

    }
    public struct arrayDataSync3
    {
        public string bsoid;
        public string createtime;
        public string updatetime;
        public string deviceid;
        public string devicetype;
        public string devicegroup;
        public string owendplatform;
        public string correlation;
        public string devicename;
        public string deviceloginname;
        public string deviceloginpassword;
        public string serverIP;
        public string RTSPport;
        public string flag;
        public string devicestate;
        public string devicenote;
    }
    #endregion

    #region JsonRequestObjects
    /// <summary>
    /// 所有模块的请求都需要指定method，指定为父类
    /// </summary>
    public class JsonRequest
    {
        // 系统版本和运行情况
        public string method { get; set; }
    }

    /// <summary>
    /// 获取系统版本，该子类无特有属性
    /// </summary>
    public class JsonObjVersion : JsonRequest
    {
        //public HedaACK m_hedaAck = new HedaACK();
        ///// <summary>
        ///// 返回的正确应答格式
        ///// </summary>
        //public HedaACK HedaAck
        //{
        //    get
        //    {
        //        m_hedaAck.Body = new Body() { retCode = "0", retMsg = "CMS 1.0.0.1" };
        //        m_hedaAck.Header = new Header() { MessageType = "MSG_SG_SYSTEM_INFO_ACK", Version = "1.0" };
        //        return m_hedaAck;
        //    }
        //    set { m_hedaAck = value; }
        //}
    }

    /// <summary>
    /// 获取系统运行情况，该子类无特有属性
    /// </summary>
    public class JsonObjRunStatus : JsonRequest
    {
    }

    public class JsonObjDeviceId : JsonRequest
    {
        public string deviceid { get; set; }
    }

    /// <summary>
    /// 用户日志查询
    /// </summary>
    /// 
    public class JsonObjUserLogSearch : JsonRequest
    {
        public string begtime { get; set; } //开始时间
        public string endtime { get; set; } //结束时间
        public string user { get; set; }    //用户信息、设备登录用户
    }

    /// <summary>
    /// 添加对接设备
    /// </summary>
    public class JsonObjAddDevice : JsonRequest
    {
        public string devicetype { get; set; }  //设备类型
        public string devicename { get; set; }  //设备名
        public string name { get; set; }    //设备名
        public string ip { get; set; }  //设备IP
        public string port { get; set; }    //设备端口
        public string user { get; set; }    //设备登录用户
        public string pwd { get; set; } //设备登录密码
        public string pin { get; set; } //设备产品型号
        public string supplier { get; set; }    //设备供应商
        public string in_channel { get; set; }  //报警主机输入端口数量
        public string out_channel { get; set; } //报警主机输出端口数量
        public string in_out { get; set; }  //门禁读卡器门内门外标记
        public string chipin_count { get; set; }    //电源时序器插口数量
        public string channel_count { get; set; }   //同录或CVR或智能分析仪通道数量

        //public HedaACK m_hedaAck = new HedaACK();
        ///// <summary>
        ///// 返回的正确应答格式
        ///// </summary>
        //public HedaACK HedaAck
        //{
        //    get
        //    {
        //        m_hedaAck.Body = new Body() { retCode = "0", retMsg = "添加设备成功" };
        //        m_hedaAck.Header = new Header() { MessageType = "MSG_SG_DEVICE_ACK_ACK", Version = "1.0" };
        //        return m_hedaAck;
        //    }
        //    set { m_hedaAck = value; }
        //}
    }

    /// <summary>
    /// 添加摄像头、麦克风
    /// </summary>
    public class JsonObjAddCamera : JsonRequest
    {
        public string devicetype { get; set; }  //设备类型
        public string devicename { get; set; }  //设备名
        public string bindid { get; set; }   //绑定设备ID
        public string groupid { get; set; }   //上级组ID
        public string loginid { get; set; }   //设备登录ID
        public string loginpwd { get; set; }   //设备登录密码
        public string ip { get; set; }  //设备IP
        public string port { get; set; }    //设备端口
        public string mainrtsp { get; set; }   //主流rtsp地址
        public string auxrtsp { get; set; }   //辅流rtsp地址
        public string flag { get; set; }   //使用主辅流标记
        public string devicestate { get; set; }   //设备状态
        public string note { get; set; }   //备注
        public string mic { get; set; }   //拾音器
        public string radio { get; set; }   //扬声器

        //public HedaACK m_hedaAck = new HedaACK();
        ///// <summary>
        ///// 返回的正确应答格式
        ///// </summary>
        //public HedaACK HedaAck
        //{
        //    get
        //    {
        //        m_hedaAck.Body = new Body()
        //        {
        //            deviceid = "CMS0001-D2000002",
        //            mainurl = "rtsp://10.10.1.194:554/CMS0001-D2000002-0_VEDIO.sdp",
        //            auxurl = "rtsp://10.10.1.194:554/CMS0001-D2000002-1_VEDIO.sdp",
        //            retCode = "0",
        //            retMsg = "添加设备成功"
        //        };
        //        m_hedaAck.Header = new Header()
        //        {
        //            MessageType = "MSG_SC_DEICE_MGR_ACK",
        //            Version = "1.0"
        //        };
        //        return m_hedaAck;
        //    }
        //    set { m_hedaAck = value; }
        //}
    }

    /// <summary>
    /// 删除对接设备
    /// </summary>
    public class JsonObjDelDevice : JsonRequest
    {
        public string deviceid { get; set; }
    }

    /// <summary>
    /// 删除摄像头、麦克风
    /// </summary>
    public class JsonObjDeleteCamera : JsonRequest
    {
        public string deviceid { get; set; }
        public string devicename { get; set; }
        public string delrecord { get; set; }

        public HedaACK m_hedaAck = new HedaACK();
        /// <summary>
        /// 返回的正确应答格式
        /// </summary>
        public HedaACK hedaAck
        {
            get
            {
                m_hedaAck.Body = new Body()
                {
                    retCode = "0",
                    retMsg = "删除设备成功"
                };
                m_hedaAck.Header = new Header()
                {
                    MessageType = "MSG_SC_DEVICE_MGR_ACK",
                    Version = "1.0"
                };
                return m_hedaAck;
            }
            set { m_hedaAck = value; }
        }
    }

    /// <summary>
    /// 修改对接设备
    /// </summary>
    public class JsonObjUpdateDevice : JsonRequest
    {
        public string deviceid { get; set; }    //设备ID
        public string devicetype { get; set; }  //设备类型
        //public string devicename { get; set; }  //设备名
        public string name { get; set; }   //绑定设备ID
        public string ip { get; set; }  //设备IP
        public string port { get; set; }    //设备端口
        public string user { get; set; }    //设备登录用户
        public string pwd { get; set; }    //设备登录密码
        public string in_channel { get; set; }  //报警主机输入端口数量
        public string out_channel { get; set; } //报警主机输出端口数量
        public string chipin_count { get; set; }    //电源时序器插口数量
        public string channel_count { get; set; }   //同录或CVR或智能分析仪通道数量

        public HedaACK m_hedaAck = new HedaACK();
        /// <summary>
        /// 返回的正确应答格式
        /// </summary>
        public HedaACK hedaAck
        {
            get
            {
                m_hedaAck.Body = new Body()
                {
                    retCode = "0",
                    retMsg = "修改设备成功"
                };
                m_hedaAck.Header = new Header()
                {
                    MessageType = "MSG_SC_DEVICE_ACT_ACK",
                    Version = "1.0"
                };
                return m_hedaAck;
            }
            set { m_hedaAck = value; }
        }

    }

    /// <summary>
    /// 修改摄像头、麦克风
    /// </summary>
    public class JsonObjUpdateCamera : JsonRequest
    {
        public string deviceid { get; set; }    //设备ID
        public string devicetype { get; set; }  //设备类型
        public string devicename { get; set; }  //设备名
        public string bindid { get; set; }   //绑定设备ID
        public string groupid { get; set; }   //上级组ID
        public string loginid { get; set; }   //设备登录ID
        public string loginpwd { get; set; }   //设备登录密码
        public string ip { get; set; }  //设备IP
        public string port { get; set; }    //设备端口
        public string mainrtsp { get; set; }   //主流rtsp地址
        public string auxrtsp { get; set; }   //辅流rtsp地址
        public string flag { get; set; }   //使用主辅流标记
        public string devicestate { get; set; }   //设备状态
        public string note { get; set; }   //备注
        public string mic { get; set; }   //拾音器
        public string radio { get; set; }   //扬声器

        //public HedaACK m_hedaAck = new HedaACK();
        ///// <summary>
        ///// 返回的正确应答格式
        ///// </summary>
        //public HedaACK hedaAck
        //{
        //    get
        //    {
        //        m_hedaAck.Body = new Body()
        //        {
        //            r_main_rtsp = "rtsp://10.10.1.194:554/CMS0001-D2000002-0_VEDIO.sdp",
        //            r_aux_rtsp = "rtsp://10.10.1.194:554/CMS0001-D2000002-1_VEDIO.sdp",
        //            retCode = "0",
        //            retMsg = "添加设备成功"
        //        };
        //        m_hedaAck.Header = new Header()
        //        {
        //            MessageType = "MSG_SC_DEICE_MGR_ACK",
        //            Version = "1.0"
        //        };
        //        return m_hedaAck;
        //    }
        //    set { m_hedaAck = value; }
        //}
    }

    /// <summary>
    /// 查询对接设备
    /// </summary>
    public class JsonObjDeviceSearch : JsonRequest
    {
        public string device { get; set; }  //设备ID或设备名
        public string devicetype { get; set; }  //设备类型
        public string isexact { get; set; } //精确查找标记
    }

    /// <summary>
    /// 查询摄像头/麦克风
    /// </summary>
    public class JsonObjCameraSearch : JsonRequest
    {
        public string platid { get; set; }  //平台ID
        public string device { get; set; }  //设备ID或设备名
        public string devicetype { get; set; }  //设备类型
        public string isexact { get; set; } //精确查找标记
    }

    /// <summary>
    /// IP段批量添加摄像头
    /// </summary>
    public class JsonObjBatAddDeviceFromIP : JsonRequest
    {
        public string devicetype { get; set; }  //设备类型
        public string addmode { get; set; }//批量添加类型"
        public string loginid { get; set; }//设备连接名"
        public string loginpwd { get; set; }//设备连接密码"
        public string rtspflag { get; set; }//录像使用主辅流标记"
        public string allurl { get; set; }//使用主辅流地址是否是全地址"
        public string mainurl { get; set; }//主流地址"
        public string auxurl { get; set; }//辅流地址"
        public string mic { get; set; }//拾音器"
        public string radio { get; set; }//扬声器"
        public List<arrayIP> array = new List<arrayIP>();  //[{"ip":"ip地址"},{"":""},...]
    }

    /// <summary>
    /// XML 批量添加摄像头
    /// </summary>
    public class JsonObjBatAddDeviceFromXml : JsonRequest
    {
        public string addmode { get; set; }//批量添加类型"
        public List<arrayXml> array = new List<arrayXml>();
    }

    /// <summary>
    /// 测试IP范围内的设备是否连接
    /// </summary>
    public class JsonObjBatPingDevice : JsonRequest
    {
        public string firstIP { get; set; }   //开始IP
        public string lastIP { get; set; }   //结束IP

    }

    /// <summary>
    /// 导出设备xml，该子类无特有属性
    /// </summary>
    public class JsonObjSaveDevFile : JsonRequest
    {
    }

    /// <summary>
    /// 获取报警主机设备供应商
    /// </summary>
    public class JsonObjAlarmSupplier : JsonRequest
    {
        public string deviceid { get; set; }    //设备ID
    }

    /// <summary>
    /// 报警主机和摄像头绑定
    /// </summary>
    public class JsonObjBindAlarmCamera : JsonRequest
    {
        public string alarmid { get; set; }   //报警主机ID
        public string areaid { get; set; }   //房间ID
        public string cameraid { get; set; }   //摄像头ID
        public string alarm_in_channel { get; set; }   //报警主机输入口
        public string alarm_out_channel { get; set; }   //报警主机输出口
    }

    /// <summary>
    /// 报警主机和摄像头解绑定.
    /// 所有属性继承JsonObjBindAlarmCamera 父类
    /// </summary>
    public class JsonObjRebindAlarmCamera : JsonObjBindAlarmCamera
    {
    }

    /// <summary>
    /// 查询报警主机和摄像头绑定信息
    /// 额外指定房间即可，其它属性继承JsonObjBindAlarmCamera 绑定类
    /// </summary>
    public class JsonObjGetAlarmCamera : JsonObjBindAlarmCamera
    {
        public string areaid { get; set; }   //房间ID
    }

    /// <summary>
    /// 获取行为分析服务器设备供应商
    /// </summary>
    public class JsonObjBASupplier : JsonRequest
    {
        public string deviceid { get; set; }    //设备ID
    }

    /// <summary>
    /// 智能分析仪通道与摄像头绑定
    /// 需要指定CameraID和通道号
    /// 继承关系（JsonObjGetBACamera：JsonObjBASupplier：JsonRequest）。
    /// </summary>
    public class JsonObjBindBACamera : JsonObjGetBACamera
    {
        public string channel { get; set; }     //通道号
    }

    /// <summary>
    /// 查询智能分析仪通道与摄像头绑定信息
    /// 需要指定CameraID，继承JsonObjBASupplier即可。
    /// </summary>
    public class JsonObjGetBACamera : JsonObjBASupplier
    {
        public string cameraid { get; set; }    //摄像头ID
    }

    /// <summary>
    /// 删除智能分析仪通道与摄像头绑定信息
    /// 删除与绑定需要的属性相同，所有属性继承之JsonObjGetBACamera
    /// </summary>
    public class JsonObjDeleteBACamera : JsonObjGetBACamera
    { }

    /// <summary>
    /// 启用和关闭智能分析仪通道与摄像头绑定信息
    /// 只需要再额外指定状态，其它属性继承JsonObjOpenBACamera即可
    /// 继承关系（JsonObjOpenBACamera:JsonObjGetBACamera:JsonObjBASupplier:JsonRequest）。
    /// </summary>
    public class JsonObjOpenBACamera : JsonObjBindBACamera
    {
        public string state { get; set; }   //状态，1-启用，0-关闭
    }

    /// <summary>
    /// 获取设备供应商
    /// </summary>
    public class JsonObjCVRSupplier : JsonRequest
    {
        public string deviceid { get; set; }    //设备ID
    }

    /// <summary>
    /// 中央存储通道与摄像头绑定
    /// 绑定需要指定摄像头和通道号，继承JsonObjGetCVRCamera
    /// </summary>
    public class JsonObjBindCVRCamera : JsonObjGetCVRCamera
    {
        public string channel { set; get; }     //通道号
    }

    /// <summary>
    /// 查询中央存储通道与摄像头绑定信息
    /// 需要指定摄像头，继承JsonObjCVRSupplier
    /// </summary>
    public class JsonObjGetCVRCamera : JsonObjCVRSupplier
    {
        public string cameraid { get; set; }    //摄像头ID
    }

    /// <summary>
    /// 删除中央存储通道与摄像头绑定信息
    /// 删除需要和查询指定相同参数，属性一致直接继承JsonObjGetCVRCamera即可
    /// </summary>
    public class JsonObjDeleteCVRCamera : JsonObjGetCVRCamera
    { }

    /// <summary>
    /// 查询录像文件列表
    /// 在查询绑定信息之后，额外指定开始和结束时间。继承查询类 JsonObjGetCVRCamera
    /// </summary>
    public class JsonObjGetCVRFileList : JsonObjGetCVRCamera
    {
        public string starttime { get; set; }   //开始时间
        public string endtime { get; set; }     //结束时间
    }

    /// <summary>
    /// 开始下载录像文件
    /// 获取列表后，指定路径和文件名即可下载，继承JsonObjGetCVRFileList查询录像文件列表类
    /// </summary>
    public class JsonObjStartDownloadFile : JsonObjGetCVRFileList
    {
        public string savepath { get; set; }   //ftp保存路径
        public string savename { get; set; }   //录像保存文件名
    }

    /// <summary>
    /// 获取下载进度
    /// 指定下载标记，其它属性继承JsonObjGetCVRCamera
    /// </summary>
    public class JsonObjGetDownloadPos : JsonObjGetCVRCamera
    {
        public string downhandle { get; set; }   //下载标记
    }

    /// <summary>
    /// 停止下载
    /// 所有属性与获取下载进度类相同，继承JsonObjGetDownloadPos
    /// </summary>
    public class JsonObjStopDownload : JsonObjGetDownloadPos
    { }

    /// <summary>
    /// 开始下载房间所有指定时间内录像文件
    /// </summary>
    public class JsonObjStartDownloadAreaFile : JsonRequest
    {
        public string areaid { get; set; }  //房间ID
        public string starttime { get; set; }   //开始时间
        public string endtime { get; set; }     //结束时间
        public string savepath { get; set; }   //ftp保存路径
    }

    /// <summary>
    /// 云台控制
    /// 命令，速度和开始关闭标记需要相应属性，继承JsonObjGetCVRCamera
    /// </summary>
    public class JsonObjPTZControl : JsonObjGetCVRCamera
    {
        public string cmd { get; set; }   //云台控制 命令
        public string speed { get; set; }   //云台控制  速度
        public string flag { get; set; }   //开始关闭标记
    }

    /// <summary>
    /// 预置点控制
    /// 指定命令和预置点序号，其它属性继承JsonObjGetCVRCamera
    /// </summary>
    public class JsonObjPTZPreset : JsonObjGetCVRCamera
    {
        public string cmd { get; set; }   //云台控制 命令
        public string index { get; set; }   //预置点序号

    }

    /// <summary>
    /// 获取解码器设备供应商
    /// </summary>
    public class JsonObjDecoderSupplier : JsonRequest
    {
        public string deviceid { get; set; }    //设备ID
    }

    /// <summary>
    /// 获取门禁设备供应商
    /// </summary>
    public class JsonObjDoorCardSupplier : JsonRequest
    {
        public string deviceid { get; set; }    //设备ID
    }

    /// <summary>
    /// 查询刷卡记录
    /// </summary>
    public class JsonObjGetDoorCardLog : JsonRequest
    {
        public string begtime { get; set; } //开始时间
        public string endtime { get; set; } //结束时间
        public string cardid { get; set; }  //门卡ID
        public string user { get; set; }    //刷卡用户名或ID
        public string area { get; set; }    //房间名或ID
        public string doorcard { get; set; }   //门禁主机名或ID
        public string cardreader { get; set; }   //门禁读卡器名或ID
    }

    /// <summary>
    /// 获取网络键盘设备供应商
    /// </summary>
    public class JsonObjNetKeySupplier : JsonRequest
    {
        public string deviceid { get; set; }    //设备ID
    }

    /// <summary>
    /// 获取电源时序器设备供应商
    /// </summary>
    public class JsonObjPSSupplier : JsonRequest
    {
        public string deviceid { get; set; }    //设备ID
    }

    /// <summary>
    /// 获取电源时序器启动配置信息
    /// </summary>
    public class JsonObjGetSequencerConf : JsonRequest
    {
        public string area { get; set; }    //房间
        public string sequencer { get; set; }   //电源时序器
    }

    /// <summary>
    /// 添加电源时序器启动配置信息
    /// </summary>
    public class JsonObjSequencerConf : JsonRequest
    {
        public string areaid { get; set; }  //房间ID
        public string sequencerid { get; set; }   //电源时序器ID
        public string bsotype { get; set; }   //业务模块类型
        public string openorder { get; set; }   //启动顺序
        public string closeorder { get; set; }   //关闭顺序
    }

    /// <summary>
    /// 删除电源时序器启动配置信息
    /// </summary>
    public class JsonObjDeleteSequencerConf : JsonRequest
    {
        public string areaid { get; set; }  //房间ID
        public string sequencerid { get; set; }   //电源时序器ID
        public string bsotype { get; set; } //业务模块类型
    }

    /// <summary>
    /// 获取同步录音录像设备供应商
    /// </summary>
    public class JsonObjSyncRecordSupplier : JsonRequest
    {
        public string deviceid { get; set; }    //设备ID
    }

    /// <summary>
    /// 同录设备通道与摄像头绑定
    /// 指定通道进行绑定，其它属性继承JsonObjGetSyncRecordCamera
    /// </summary>
    public class JsonObjBindSyncRecordCamera : JsonObjGetSyncRecordCamera
    {
        public string channel { get; set; }     //通道号
    }

    /// <summary>
    /// 查询同录通道与摄像头绑定信息
    /// </summary>
    public class JsonObjGetSyncRecordCamera : JsonObjSyncRecordSupplier
    {
        public string cameraid { get; set; }    //摄像头ID 
    }

    /// <summary>
    /// 删除同录通道与摄像头绑定信息
    /// 继承查询Obj类 JsonObjGetSyncRecordCamera
    /// </summary>
    public class JsonObjDeleteSyncRecordCamera : JsonObjGetSyncRecordCamera
    { }

    /// <summary>
    /// 添加房间
    /// </summary>
    public class JsonObjAddArea : JsonRequest
    {
        public string areaname { get; set; }   //房间名
        public string areatype { get; set; }   //房间类型
        public string note { get; set; }    //备注
    }

    /// <summary>
    /// 删除房间
    /// </summary>
    public class JsonObjDelArea : JsonRequest
    {
        public string areaid { get; set; }  //房间ID
    }

    /// <summary>
    /// 修改房间
    /// 指定房间ID，其它属性继承JsonObjAddArea
    /// </summary>
    public class JsonObjUpdateArea : JsonObjAddArea
    {
        public string areaid { get; set; }  //房间ID
    }

    /// <summary>
    /// 查询房间
    /// </summary>
    public class JsonObjGetArea : JsonRequest
    {
        public string area { get; set; }    //房间ID或者房间名
        public string isexact { get; set; } //精确查找标记
    }

    /// <summary>
    /// 绑定房间和设备
    /// 指定设备信息，继承查询类 JsonObjGetAreaDevice
    /// </summary>
    public class JsonObjBindAreaDevice : JsonObjGetAreaDevice
    {
        private List<arrayArea> m_array = new List<arrayArea>();
        public List<arrayArea> array
        {
            get { return m_array; }
            set
            {
                array = value;
            }
        }    //
    }

    /// <summary>
    /// 查询房间关联对接设备信息
    /// </summary>
    public class JsonObjGetAreaDevice : JsonRequest
    {
        public string areaid { get; set; }  //房间ID
    }

    /// <summary>
    /// 查询房间摄像头信息
    /// 指定房间名，继承查询类 JsonObjGetAreaDevice
    /// </summary>
    public class JsonObjGetAreaCamera : JsonObjGetAreaDevice
    {
        public string areaname { get; set; }    //房间ID或房间名模糊查询
    }

    /// <summary>
    /// 添加用户
    /// </summary>
    public class JsonObjAddUser : JsonObjUpdateUser
    {
        public string pwd { get; set; }     //用户密码
    }

    /// <summary>
    /// 修改用户(包括权限)
    /// </summary>
    public class JsonObjUpdateUser : JsonRequest
    {
        public string userid { get; set; }   //用户ID
        public string username { get; set; }   //真实姓名
        public string telephone { get; set; }   //办公室电话
        public string mobile { get; set; }      //移动电话
        public string email { get; set; }   //电子邮件
                                            // public string roleid { get; set; }   //角色ID
        private List<arrayUser> aa = new List<arrayUser>();
        public List<arrayUser> array { get { return aa; } set { } }    //角色列表，可以没有此属性
    }

    /// <summary>
    /// 修改用户资料(个人中心修改自身信息)
    /// 所有属性继承添加用户类，JsonObjAddUser
    /// </summary>
    public class JsonObjUpdateUserInfo : JsonObjAddUser
    { }

    /// <summary>
    /// 删除用户
    /// </summary>
    public class JsonObjDeleteUser : JsonRequest
    {
        public string userid { get; set; }   //用户ID
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    public class JsonObjUserLogin : JsonRequest
    {
        public string userid { get; set; }   //用户ID
        public string pwd { get; set; }     //用户登录密码，MD5加密后的
        public string flag { get; set; }    //0-默认登录查询所有权限，1-winform客户端登录。

    }

    /// <summary>
    /// 用户信息查询
    /// 所有属性继承删除用户类，JsonObjDeleteUser
    /// </summary>
    public class JsonObjUserSearch : JsonObjDeleteUser
    { }

    /// <summary>
    /// 校验用户ID
    /// </summary>
    public class JsonObjCheckUserID : JsonRequest
    {
        public string begtime { get; set; }     //开始时间，格式：YYYY-MM-DD
        public string endtime { get; set; }     //结束时间，格式：YYYY-MM-DD
        public string user { get; set; }        //用户名或用户ID
    }

    /// <summary>
    /// 重置密码
    /// 所有属性继承删除用户类，JsonObjDeleteUser
    /// </summary>
    public class JsonObjResetUserPwd : JsonRequest
    { }


    public class JsonObjGetUser : JsonRequest
    {
        public string user { get; set; }        //用户ID/用户名/角色名，等于空字符串时查询所有用户
        public string flag { get; set; }        //1-winform,0或空查询所有权限   
        public string isexact { get; set; }     //1-精确查询，0-模糊查询
    }

    /// <summary>
    /// 修改用户密码
    /// 所有属性继承用户登录类，JsonObjDeleteUserary>
    public class JsonObjUpdatePwd : JsonObjUserLogin
    {
        public string oldpwd { get; set; }   //旧密码
    }

    /// <summary>
    /// 绑定用户角色
    /// 额外指定角色列表，用户ID属性继承JsonObjDeleteUser类
    /// </summary>
    public class JsonObjBindUserRole : JsonObjDeleteUser
    {
        public List<arrayUser> array = new List<arrayUser>();    //角色列表
    }

    /// <summary>
    /// 添加权限
    /// </summary>
    public class JsonObjAddPrivilege : JsonRequest
    {
        public string privilegename { get; set; }   //权限名(唯一)

        private List<arrayPrivilege> m_array = new List<arrayPrivilege>();

        public List<arrayPrivilege> array
        {
            get { return m_array; }
            set { m_array = value; }
        }
    }

    /// <summary>
    /// 修改权限
    /// 指定privilegeid，继续添加权限类 JsonObjAddPrivilege
    /// </summary>
    public class JsonObjUpdatePrivilege : JsonObjAddPrivilege
    {
        public string privilegeid { get; set; } //权限ID
    }

    public class JsonObjDeletePrivilege : JsonRequest
    {
        public string privilegeid { get; set; } //权限ID
        public string privilegename { get; set; }   //权限名(唯一)
    }

    public class JsonObjRolePrivilege : JsonRequest
    {
        public string roleid { get; set; }  //角色ID
    }

    public class JsonObjPrivilegeDevice : JsonRequest
    {
        public string privilegeid { get; set; } //权限ID
    }

    public class JsonObjGetPrivilege : JsonRequest
    {
        public string flag { get; set; }    //0-返回全部，1-winform客户端权限
        public string privilege { get; set; }   //权限ID/权限名/设备名，等于空字符串时查询所有权限
        public string isexact { get; set; } //1-精确查询，0-模糊查询
    }

    /// <summary>
    /// 添加角色
    /// </summary>
    public class JsonObjAddRole : JsonRequest
    {
        public string rolename { get; set; }        //角色名

        private List<arrayRole> m_array = new List<arrayRole>();    //添加角色时的权限集合

        public List<arrayRole> array
        {
            get { return m_array; }
            set { m_array = value; }
        }
    }

    /// <summary>
    /// 修改角色
    /// 额外指定权限ID，继承添加角色类即可JsonObjAddRole
    /// </summary>
    public class JsonObjUpdateRole : JsonObjAddRole
    {
        public string roleid { get; set; }  //角色ID
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    public class JsonObjDeleteRole : JsonRequest
    {
        public string rolename { get; set; }        //角色名
        public string roleid { get; set; }  //角色ID
    }

    /// <summary>
    /// 获取指定用户的所有角色
    /// </summary>
    public class JsonObjUserRole : JsonRequest
    {
        public string userid { get; set; }  //用户登录ID
    }

    /// <summary>
    /// 获取所有角色
    /// </summary>
    public class JsonObjGetRole : JsonRequest
    {
        public string role { get; set; }    //角色ID/角色名/权限名，等于空字符串时查询所有角色
        public string isexact { get; set; }    //是否精确查询
    }

    public class JsonObjAddPlat : JsonRequest
    {
        public string remoteip { get; set; }    //下级平台添加
        public string port { get; set; }    //远程平台端口
    }

    /// <summary>
    /// 修改子平台
    /// 修改指定IDr 子平台，需要修改IP和PORT。继承JsonObjDeletePlat
    /// </summary>
    public class JsonObjUpdatePlat : JsonObjDeletePlat
    {
        public string ip { get; set; }  //新IP地址
        public string port { get; set; }    //新端口
    }

    /// <summary>
    /// 删除平台
    /// </summary>
    public class JsonObjDeletePlat : JsonRequest
    {
        public string platid { get; set; }  //子平台ID
    }

    /// <summary>
    /// 条件获取平台列表
    /// </summary>
    public class JsonObjPlatListExact : JsonRequest
    {
        public string plat { get; set; }  //名称或者ID
        public string isexact { get; set; }  //是否精确查询
    }

    /// <summary>
    /// 添加组
    /// </summary>
    public class JsonObjAddGroup : JsonRequest
    {
        public string groupname { get; set; }   //组名
        public string parentgroupid { get; set; }   //上级组ID
    }

    /// <summary>
    /// 修改组
    /// 需要指定group id，继承JsonObjAddGroup
    /// </summary>
    public class JsonObjUpdateGroup : JsonObjAddGroup
    {
        public string groupid { get; set; }   //组ID
    }

    /// <summary>
    /// 删除组
    /// </summary>
    public class JsonObjDeleteGroup : JsonRequest
    {
        public string groupid { get; set; }   //组ID
    }

    /// <summary>
    /// 获取平台下所有组
    /// </summary>
    public class JsonObjPlatGroup : JsonRequest
    {
        public string platid { get; set; }   //平台ID
    }

    /// <summary>
    /// 组查询
    /// </summary>
    public class JsonObjGroupSearch : JsonRequest
    {
        public string group { get; set; }   //组ID或组名
        public string isexact { get; set; } //组ID或组名
    }

    /// <summary>
    /// 云台 移动
    /// deviceid属性继承 JsonObjGetPresets类
    /// </summary>
    public class JsonObjKeepMove : JsonObjGetPresets
    {
        public string x { get; set; }    //x轴移动速度
        public string y { get; set; }    //y轴移动速度
        public string z { get; set; }    //调整变焦
    }

    /// <summary>
    /// 云台 停止移动
    /// deviceid属性继承 JsonObjGetPresets类
    /// </summary>
    public class JsonObjStopMove : JsonObjGetPresets
    {
        public string xyz { get; set; }    //值是z时停止调焦，值是x,y时停止移动
    }

    /// <summary>
    /// 云台 相对移动
    /// 所有属性继承 云台移动类JsonObjKeepMove
    /// </summary>
    public class JsonObjRltMove : JsonObjKeepMove
    { }

    /// <summary>
    /// 获取预置点
    /// </summary>
    public class JsonObjGetPresets : JsonRequest
    {
        public string deviceid { get; set; }    //设备ID
    }

    /// <summary>
    /// 设置预置点，删除和移动
    /// </summary>
    public class JsonObjSetPresets : JsonObjGetPresets
    {
        public string name { get; set; }    //预置点名
        public string Token { get; set; }    //预置点token，如果是新增预置点，此处设置””或”-1”
    }

    /// <summary>
    /// RTSP转发和批量转发
    /// </summary>
    public class JsonObjRtspRelay : JsonRequest
    {
        public string deviceid { get; set; }    //(下级平台+)设备ID,多个设备ID以|分割,不支持下级平台设备的批量转发请求
        public string flag { get; set; }    //主辅流标记
    }

    /// <summary>
    /// 录像文件回放
    /// </summary>
    public class JsonObjTSFileRelay : JsonRequest
    {
        public string deviceid { get; set; }    //(下级平台+)设备ID
        public string begtime { get; set; }    //开始时间
        public string endtime { get; set; }    //结束时间
    }

    /// <summary>
    /// 回放进度调整
    /// </summary>
    public class JsonObjTSFileSeekPlay : JsonRequest
    {
        public string deviceid { get; set; }    //设备ID
        public string videoname { get; set; }    //视频名
        public string videopos { get; set; }    //播放时间点
    }

    /// <summary>
    /// 获取视频文件下载列表
    /// 属性与录像文件回放相同，继承JsonObjTSFileRelay
    /// </summary>
    public class JsonObjDownloadFileList : JsonObjTSFileRelay
    { }

    /// <summary>
    /// 获取设备转发地址(内部使用)
    /// 指定是否要音频，继承RTSP转发类JsonObjRtspRelay
    /// </summary>
    public class JsonObjGetRelayRtsp : JsonObjRtspRelay
    {
        public string audio { get; set; }   //是否带有音频
    }

    /// <summary>
    /// 添加、修改和批量添加计划三个子类的父类
    /// </summary>
    public class JsonObjPlan : JsonRequest
    {
        public string deviceid { get; set; }   //设备ID
        public string begdate { get; set; }   //开始日期
        public string enddate { get; set; }   //结束日期
        public string begloop { get; set; }   //开始循环标记
        public string endloop { get; set; }   //结束循环标记
        public string begtime { get; set; }   //开始时间
        public string endtime { get; set; }   //结束时间
        public string loopflag { get; set; }   //循环标志
        public string tasktype { get; set; }   //任务类型
        public string userid { get; set; }   //创建者ID
    }

    /// <summary>
    /// 添加和修改计划
    /// 指定单个PlanName，其它所有属性继承JsonObjPlan
    /// </summary>
    public class JsonObjAddPlan : JsonObjPlan
    {
        public string planname { get; set; }   //计划名
    }

    /// <summary>
    /// 查询计划
    /// </summary>
    public class JsonObjPlanSearch : JsonRequest
    {
        public string plan { get; set; }    //查询条件，可以是计划名、设备名、设备ID
    }

    /// <summary>
    /// 精确查询计划
    /// 与删除计划相同属性，继承JsonObjDeletePlan
    /// </summary>
    public class JsonObjExactPlanSearch : JsonObjDeletePlan
    {
    }

    /// <summary>
    /// 删除计划
    /// </summary>
    public class JsonObjDeletePlan : JsonRequest
    {
        public string deviceid { get; set; }   //设备ID        
        public string planname { get; set; }   //计划名
    }

    /// <summary>
    /// 启动、关闭计划
    /// 指定Status，DeviceID和PlanName继承 删除计划类JsonObjDeletePlan
    /// </summary>
    public class JsonObjClosePlan : JsonObjDeletePlan
    {
        public string state { get; set; }   //状态 
    }

    /// <summary>
    /// 批量添加计划任务
    /// </summary>
    public class JsonObjBatAddPlan : JsonObjPlan
    {
        private List<arrayPlan> m_array = new List<arrayPlan>();
        public List<arrayPlan> array
        {
            get { return m_array; }
            set { m_array = value; }
        }
    }

    /// <summary>
    /// 视频预览
    /// </summary>
    public class JsonObjMP4Preview : JsonRequest
    {
        public string url { get; set; } //rtsp地址
        public string loginid { get; set; } //设备登录ID
        public string loginpwd { get; set; } //设备登录密码
    }

    /// <summary>
    /// 视频预览(下级平台设备)
    /// </summary>
    public class JsonObjDevicePreview : JsonRequest
    {
        public string deviceid { get; set; } //设备ID
        public string flag { get; set; } //主辅流标记
    }

    /// <summary>
    /// 录像服务器(RMS)配置
    /// </summary>
    public class JsonObjRMSConf : JsonRequest
    {
        public string ip { get; set; } //服务器IP
        public string port { get; set; } //端口
        public string retain_time { get; set; } //循环删除录像时间
        public string src_num { get; set; } //最大接入设备数量
        public string reconn_time { get; set; } //资源断线重连时间
        public string save_type { get; set; } //保存类型
        public string length { get; set; } //单媒体文件长度
        public string section { get; set; } //切片数量
        public string save_pos { get; set; } //保持位置
        public string local_path { get; set; } //本地路径
        public string leave_space { get; set; } //最大使用空间
        public string over_opt { get; set; } //采取的操作
    }

    /// <summary>
    /// 数据库配置,
    /// </summary>
    public class JsonObjDBConf : JsonRequest
    {
        public string dbname { get; set; } //数据库名
        public string ip { get; set; } //服务器IP
        public string port { get; set; } //端口
        public string dbuser { get; set; } //用户名
        public string dbpwd { get; set; } //密码
    }

    /// <summary>
    /// 测试数据库连接，所有属性继承JsonObjDBConf
    /// </summary>
    public class JsonObjCheckDB : JsonObjDBConf
    { }

    /// <summary>
    /// 重启服务
    /// </summary>
    public class JsonObjReStart : JsonRequest
    {
        //重启的服务器，ALL/CMS/HDR/RMS, ALL-重启所有服务、CMS-CMS服务器、HDR-转发服务器、RMS-录像服务器
        public string server { get; set; }
    }

    /// <summary>
    /// 同步所有数据(内部使用)
    /// </summary>
    public class JsonObjAllDataSync : JsonRequest
    {
        public string count { get; set; }  //同步表数量、现最大是3，分别是平台、组、设备表
        public string table_1 { get; set; }  //表1表名
        public string table_2 { get; set; }  //表1表名
        public string table_3 { get; set; }  //表1表名
        private List<arrayDataSync1> m_array_1 = new List<arrayDataSync1>();
        private List<arrayDataSync2> m_array_2 = new List<arrayDataSync2>();
        private List<arrayDataSync3> m_array_3 = new List<arrayDataSync3>();
        public List<arrayDataSync1> array_1
        {
            get { return m_array_1; }
            set { m_array_1 = value; }
        }
        public List<arrayDataSync2> array_2
        {
            get { return m_array_2; }
            set { m_array_2 = value; }
        }

        public List<arrayDataSync3> array_3
        {
            get { return m_array_3; }
            set { m_array_3 = value; }
        }
    }

    /// <summary>
    /// 同步部分数据(内部使用)
    /// </summary>
    public class JsonObjDataSync : JsonRequest
    {
        public string count { get; set; }  //同步表数量、现最大是3，分别是平台、组、设备表
        public string opt { get; set; } //数据操作类型
        public string table_1 { get; set; }  //表1表名
        public List<string> array_1 { get; set; }  //
    }
    #endregion

    #region Json ACK 待定，尚未使用
    public class Body
    {
        /// <summary>
        /// 错误码，0-成功、1-操作失败
        /// </summary>
        public string retCode { get; set; }
        /// <summary>
        /// 返回具体的错误信息
        /// </summary>
        public string retMsg { get; set; }
        //CPU使用率（百分比）
        public string cpu_percent { get; set; }
        /// <summary>
        /// 内存使用率（百分比）
        /// </summary>
        public string mem_percent { get; set; }
        /// <summary>
        /// 内存使用量（MB）
        /// </summary>
        public string mem_usage { get; set; }
        /// <summary>
        /// 新添加设备ID，错误时不返回此属性
        /// </summary>
        public string deviceid { get; set; }
        /// <summary>
        /// 转发后的主流地址
        /// </summary>
        public string mainurl { get; set; }
        /// <summary>
        /// 转发后的辅流地址
        /// </summary>
        public string auxurl { get; set; }
        /// <summary>
        /// 修改设备后的新的辅流转发地址
        /// </summary>
        public string r_aux_rtsp { get; set; }

        /// <summary>
        /// 修改设备后的新的主流转发地址
        /// </summary>
        public string r_main_rtsp { get; set; }
    }

    public class Header
    {
        public string MessageType { get; set; }
        public string Version { get; set; }
    }

    public class HedaACK
    {
        public Body Body { get; set; }
        public Header Header { get; set; }
    }

    public class JsonAckObject
    {
        public HedaACK HedaACK { get; set; }
    }
    #endregion
}
