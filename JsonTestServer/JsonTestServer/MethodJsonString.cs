using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        BatAddDevice,   //摄像头批量处理-IP段添加
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

    public class JsonRequest
    {
        // 系统版本和运行情况
        public string method { get; set; }

        #region 用户日志查询
        public string begtime { get; set; } //开始时间
        public string endtime { get; set; } //结束时间
        public string user { get; set; }    //用户信息、设备登录用户
        #endregion

        #region 通用接口
        #region  添加对接设备
        public string devicetype { get; set; }  //设备类型
        public string devicename { get; set; }  //设备名
        public string name { get; set; }    //设备名
        public string ip { get; set; }  //设备IP
        public string port { get; set; }    //设备端口
        public string pwd { get; set; } //设备登录密码
        public string pin { get; set; } //设备产品型号
        public string supplier { get; set; }    //设备供应商
        public string in_channel { get; set; }  //报警主机输入端口数量
        public string out_channel { get; set; } //报警主机输出端口数量
        public string in_out { get; set; }  //门禁读卡器门内门外标记
        public string chipin_count { get; set; }    //电源时序器插口数量
        public string channel_count { get; set; }   //同录或CVR或智能分析仪通道数量
        #endregion
        #region 添加摄像头、麦克风
        public string bindid { get; set; }   //绑定设备ID
        public string groupid { get; set; }   //上级组ID
        public string loginid { get; set; }   //设备登录ID
        public string loginpwd { get; set; }   //设备登录密码
        public string mainrtsp { get; set; }   //主流rtsp地址
        public string auxrtsp { get; set; }   //辅流rtsp地址
        public string flag { get; set; }   //使用主辅流标记
        public string devicestate { get; set; }   //设备状态
        public string note { get; set; }   //备注
        public string mic { get; set; }   //拾音器
        public string radio { get; set; }   //扬声器
        #endregion
        #region 删除摄像头、麦克风
        public string delrecord { get; set; }   //删除录像文件
        #endregion
        #region 查询对接设备
        public string isexact { get; set; }   //精确查找标记
        #endregion
        #region 摄像头批量处理
        public string array { get; set; }   //[{"ip":"ip地址"},{"":""},...]
        public string addmode { get; set; }   //批量添加类型
        public string rtspflag { get; set; }   //录像使用主辅流标记
        public string allurl { get; set; }   //使用主辅流地址是否是全地址
        public string mainurl { get; set; }   //主流地址
        public string auxurl { get; set; }   //辅流地址
        #endregion
        #region 测试摄像头设备是否连接
        public string firstIP { get; set; }   //开始IP
        public string lastIP { get; set; }   //结束IP
        #endregion
        #endregion

        #region 报警主机
        public string alarmid { get; set; }   //报警主机ID
        public string cameraid { get; set; }   //摄像头ID
        public string alarm_in_channel { get; set; }   //报警主机输入口
        public string alarm_out_channel { get; set; }   //报警主机输出口
        #endregion

        #region 行为分析服务器
        public string channel { get; set; } //通道号
        #endregion

        #region 中央存储服务器(CVR)
        public string starttime { get; set; } //通道号
        public string savepath { get; set; }   //ftp保存路径
        public string savename { get; set; }   //录像保存文件名
        public string downhandle { get; set; }   //下载标记
        public string cmd { get; set; }   //云台控制 命令
        public string speed { get; set; }   //云台控制  速度
        public string index { get; set; }   //预置点序号
        #endregion

        #region 门禁
        public string doorcard { get; set; }   //门禁主机名或ID
        public string cardreader { get; set; }   //门禁读卡器名或ID
        #endregion

        #region 电源时序器
        public string sequencer { get; set; }   //电源时序器
        public string sequencerid { get; set; }   //电源时序器ID
        public string bsotype { get; set; }   //业务模块类型
        public string openorder { get; set; }   //启动顺序
        public string closeorder { get; set; }   //关闭顺序
        #endregion

        #region 房间操作
        public string areaid { get; set; }   //房间ID
        public string areaname { get; set; }   //房间名
        public string areatype { get; set; }   //房间类型
        public string area { get; set; }   //房间ID或房间名
        #endregion

        #region 用户管理
        public string userid { get; set; }   //用户ID
        public string username { get; set; }   //真实姓名
        public string telephone { get; set; }   //办公室电话
        public string email { get; set; }   //电子邮件
        public string roleid { get; set; }   //角色ID
        #endregion
    }

    #region Json ACK
    public class Body
    {
        public string retCode { get; set; }
        public string retMsg { get; set; }
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
