using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JsonTestServer
{
    public enum JsonRequestType
    {
        hedajwreq,      // "api/hedajwreq"
        hedacmdreq,     // "api/" 摄像头批量处理
        upload,         //  "xml/upload"
    }

    public enum JsonMethodType
    {
        Version,    //获取系统版本
        RunStatus,  //获取系统运行情况
        UserLogSearch,  //用户日志查询
        AddDevice,  //添加对接设备
        AddCamera,  //添加摄像头/麦克风
        DelDevice,  //删除对接设备
        DeleteCamera,   //删除摄像头/麦克风
        UpdateDevice,   //修改对接设备
        UpdateCamera,   //修改摄像头/麦克风
        DeviceSearch,   //查询对接设备
        CameraSearch,   //查询摄像头/麦克风
        BatAddDeviceFromIP,   //摄像头批量处理-IP段添加
        BatAddDeviceFromXml,   //摄像头批量处理-IP段添加
        BatAddDeviceImport,
        BatPingDevice,  //摄像头批量处理-测试设备是否连接
        SaveDevFile,    //摄像头批量处理-导出设备xml
        AlarmSupplier,  //获取“报警主机”设备供应商
        BindAlarmCamera,    //报警主机和摄像头绑定
        RebindAlarmCamera,  //报警主机和摄像头解绑定
        GetAlarmCamera, //查询报警主机和摄像头绑定信息
        BASupplier,     //获取“行为分析服务器”设备供应商
        BindBACamera,    //智能分析仪通道与摄像头绑定
        GetBACamera,     //查询智能分析仪通道与摄像头绑定信息
        DeleteBACamera,     //删除智能分析仪通道与摄像头绑定信息
        OpenBACamera,     //启用和关闭智能分析仪通道与摄像头绑定信息
        CVRSupplier,     //获取“中央存储服务器(CVR)”设备供应商
        BindCVRCamera,     //中央存储通道与摄像头绑定
        GetCVRCamera,     //查询中央存储通道与摄像头绑定信息
        DeleteCVRCamera,     //删除中央存储通道与摄像头绑定信息
        GetCVRFileList,     //查询录像文件列表
        StartDownloadFile,     //开始下载录像文件
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
    }

    /// <summary>
    /// 所有模块的请求都需要指定method，指定为父类
    /// </summary>
    public class JsonRequest
    {
        // 系统版本和运行情况
        public string method { get; set; }
    }

    #region JsonRequestObjects
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
        //            cpu_percent = "0",
        //            mem_percent = "0",
        //            mem_usage = "47.8MB",
        //            retCode = "0",
        //            retMsg = "系统信息查询成功"
        //        };
        //        m_hedaAck.Header = new Header()
        //        {
        //            MessageType = "MSG_SG_USER_MGR_ACK",
        //            Version = "1.0"
        //        };
        //        return m_hedaAck;
        //    }
        //    set { m_hedaAck = value; }
        //}
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
        public string oldpwd { get; set;}   //旧密码
    }

    /// <summary>
    /// 绑定用户角色
    /// 额外指定角色列表，用户ID属性继承JsonObjDeleteUser类
    /// </summary>
    public class JsonObjBindUserRole : JsonObjDeleteUser
    {
        public List<arrayUser> array { get; set; }    //角色列表
    }
    #endregion

    #region Json ACK
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
