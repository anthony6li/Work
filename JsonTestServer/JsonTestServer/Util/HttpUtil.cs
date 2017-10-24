using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Threading;
using JsonTestServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Util
{
    class HttpUtil
    {
        CookieContainer cookie = new CookieContainer();

        public string HttpPost(string Url, string postDataStr)
        {
            string retString = string.Empty;
            HttpWebRequest request = null;
            try
            {
                //Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                //byte[] postDataByte = encode.GetBytes(postDataStr);
                byte[] postDataByte = System.Text.Encoding.Default.GetBytes(postDataStr);

                request = (HttpWebRequest)WebRequest.Create(Url);
                Thread.Sleep(250);
                request.Method = "POST";
                request.ContentType = "multi-part form data";
                request.ContentLength = postDataByte.Length;
                Stream myRequestStream = request.GetRequestStream();
                myRequestStream.Write(postDataByte, 0, postDataByte.Length);
                myRequestStream.Close();

                request.Timeout = 10000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream myResponseStream = response.GetResponseStream();
                //StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.Default);
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                //JObject jo = JObject.Parse(retString);
                //string[] values = jo.Properties().Select(item => item.Value.ToString()).ToArray();

                return retString;
            }
            catch (Exception ex)
            {
                if (request != null)
                {
                    request.Abort();
                }
                MessageBox.Show(ex.Message);
                return "";
            }
            finally
            {
            }

        }

        public string HttpPostUTF8(string Url, string postDataStr)
        {
            string retString = string.Empty;
            HttpWebRequest request = null;
            try
            {
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                byte[] postDataByte = encode.GetBytes(postDataStr);

                request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postDataByte.Length;
                Stream myRequestStream = request.GetRequestStream();
                myRequestStream.Write(postDataByte, 0, postDataByte.Length);
                myRequestStream.Close();

                request.Timeout = 10000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch (Exception ex)
            {
                if (request != null)
                {
                    request.Abort();
                }
                MessageBox.Show(ex.Message);
                return "";
            }

        }

        public string HttpGet(string Url, string postDataStr)
        {
            string retString = string.Empty;
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                request.Timeout = 10000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                //StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch (Exception ex)
            {
                if (request != null)
                {
                    request.Abort();
                }
                MessageBox.Show(ex.Message);
                return "";
            }

        }

        /// <summary>
        /// 通过对应的Json对象返回对应的Json String,内容为Request说明
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected internal string JsonDetailStringCreator(string type)
        {
            string temp = string.Empty;
            try
            {
                //通过TreeView节点的Name，获取对应的枚举值
                JsonMethodType jsonMethodType = (JsonMethodType)Enum.Parse(typeof(JsonMethodType), type);
                JsonSerializerSettings jsonSetting = new JsonSerializerSettings();
                //解析时忽略Null Value的属性
                jsonSetting.NullValueHandling = NullValueHandling.Ignore;
                //可以解析继承类
                jsonSetting.TypeNameHandling = TypeNameHandling.Auto;
                jsonSetting.ConstructorHandling = ConstructorHandling.Default;
                jsonSetting.Formatting = Formatting.None;
                jsonSetting.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //object tempObj= null;
                string tempJsonStr = string.Empty;
                switch (jsonMethodType)
                {
                    case JsonMethodType.Version:
                    case JsonMethodType.RunStatus:
                    case JsonMethodType.PlatList:
                    case JsonMethodType.PlatInfo:
                    case JsonMethodType.RmsFtpInfo:
                    case JsonMethodType.AllPlanSearch:
                    case JsonMethodType.StopPreview:
                    case JsonMethodType.GetRMSConf:
                    case JsonMethodType.GetDBConf:
                    case JsonMethodType.ReqDataSync:
                    case JsonMethodType.SaveDevFile:
                        JsonObjVersion joRS = new JsonObjVersion();
                        joRS.method = type;
                        tempJsonStr = JsonConvert.SerializeObject(joRS, jsonSetting);
                        break;
                    case JsonMethodType.SysInfo:
                        //http请求路径：http://[ip]:[port]/api/sysinfo
                        break;
                    case JsonMethodType.UserLogSearch:
                        JsonObjUserLogSearch jrULS = new JsonObjUserLogSearch();
                        jrULS.method = type;
                        jrULS.begtime = "开始时间，格式：YYYY-MM-DD，可以为空值";
                        jrULS.endtime = "结束时间，格式：YYYY-MM-DD，可以为空值";
                        jrULS.user = "用户名或用户ID，可以为空值";
                        tempJsonStr = JsonConvert.SerializeObject(jrULS, jsonSetting);
                        break;
                    case JsonMethodType.AddDevice:
                        JsonObjAddDevice joAD = new JsonObjAddDevice();
                        joAD.method = type;
                        joAD.devicetype = "设备类型";
                        joAD.name = "设备名";
                        joAD.ip = "设备IP";
                        joAD.port = "设备端口";
                        joAD.user = "设备登录用户";
                        joAD.pwd = "设备登录密码";
                        joAD.pin = "设备产品型号";
                        joAD.supplier = "设备供应商";
                        joAD.in_channel = "报警主机输入端口数量";
                        joAD.out_channel = "报警主机输出端口数量";
                        joAD.in_out = "门禁读卡器门内门外标记";
                        joAD.chipin_count = "电源时序器插口数量";
                        joAD.channel_count = "同录或CVR或智能分析仪通道数量";
                        tempJsonStr = JsonConvert.SerializeObject(joAD, jsonSetting);
                        break;
                    case JsonMethodType.AddCamera:
                        JsonObjAddCamera joAC = new JsonObjAddCamera();
                        joAC.method = type;
                        joAC.devicetype = "设备类型";
                        joAC.devicename = "设备名当前系统唯一";
                        joAC.bindid = "绑定设备ID";
                        joAC.groupid = "上级组ID,没有上级组此属性为空或不填写此属性，后台添加时默认是未分组";
                        joAC.loginid = "设备登录ID";
                        joAC.loginpwd = "设备登录密码";
                        joAC.ip = "设备IP";
                        joAC.port = "设备端口";
                        joAC.mainrtsp = "主流地址";
                        joAC.auxrtsp = "辅流地址";
                        joAC.flag = "使用主辅流标记";
                        joAC.devicestate = "设备状态";
                        joAC.note = "备注";
                        joAC.mic = "是否连接拾音器1-连接、0-未连接";
                        joAC.radio = "是否连接扬声器1-连接、0-未连接";
                        tempJsonStr = JsonConvert.SerializeObject(joAC, jsonSetting);
                        break;
                    case JsonMethodType.DelDevice:
                        JsonObjDelDevice joDD = new JsonObjDelDevice();
                        joDD.method = type;
                        joDD.deviceid = "设备唯一标识，批量删除时ID之间|分割";
                        tempJsonStr = JsonConvert.SerializeObject(joDD, jsonSetting);
                        break;
                    case JsonMethodType.DeleteCamera:
                        JsonObjDeleteCamera joDC = new JsonObjDeleteCamera();
                        joDC.method = type;
                        joDC.deviceid = "设备ID，批量删除时可以是多个ID，设备ID使用|分割";
                        joDC.devicename = "设备名";
                        joDC.delrecord = "是否删除录像文件，1-删除、0-不删除";
                        tempJsonStr = JsonConvert.SerializeObject(joDC, jsonSetting);
                        break;
                    case JsonMethodType.UpdateDevice:
                        JsonObjUpdateDevice joUD = new JsonObjUpdateDevice();
                        joUD.method = type;
                        joUD.deviceid = "设备ID";
                        joUD.devicetype = "设备类型";
                        joUD.name = "设备名";
                        joUD.ip = "设备IP";
                        joUD.port = "设备端口";
                        joUD.user = "设备登录用户";
                        joUD.pwd = "设备登录密码";
                        joUD.in_channel = "报警主机输入端口数量";
                        joUD.out_channel = "报警主机输出端口数量";
                        joUD.chipin_count = "电源时序器插口数量";
                        joUD.channel_count = "同录或CVR或智能分析仪通道数量";
                        tempJsonStr = JsonConvert.SerializeObject(joUD, jsonSetting);
                        break;
                    case JsonMethodType.UpdateCamera:
                        JsonObjUpdateCamera joUC = new JsonObjUpdateCamera();
                        joUC.method = type;
                        joUC.deviceid = "设备ID";
                        joUC.devicetype = "设备类型";
                        joUC.devicename = "设备名";
                        joUC.bindid = "绑定设备ID";
                        joUC.groupid = "上级组ID";
                        joUC.loginid = "设备登录ID";
                        joUC.loginpwd = "设备登录密码";
                        joUC.ip = "设备IP";
                        joUC.port = "设备端口";
                        joUC.mainrtsp = "主流地址";
                        joUC.auxrtsp = "辅流地址";
                        joUC.flag = "使用主辅流标记";
                        joUC.devicestate = "设备状态";
                        joUC.note = "备注";
                        joUC.mic = "是否连接拾音器1-连接、0-未连接";
                        joUC.radio = "是否连接扬声器1-连接、0-未连接";
                        tempJsonStr = JsonConvert.SerializeObject(joUC, jsonSetting);
                        break;
                    case JsonMethodType.DeviceSearch:
                        JsonObjDeviceSearch joDS = new JsonObjDeviceSearch();
                        joDS.method = type;
                        joDS.device = "设备ID或设备名，为空时查询所有有效的设备";
                        joDS.devicetype = "设备类型";
                        joDS.isexact = "为1时精确查找，0-模糊查找，精确查询时device必须为设备ID";
                        tempJsonStr = JsonConvert.SerializeObject(joDS, jsonSetting);
                        break;
                    case JsonMethodType.CameraSearch:
                        JsonObjCameraSearch joCS = new JsonObjCameraSearch();
                        joCS.method = type;
                        joCS.platid = "平台ID，搜索指定平台下的设备，当属性为空时查询当前平台下的设备";
                        joCS.devicetype = "设备类型";
                        joCS.devicetype = "设备ID或设备名";
                        joCS.isexact = "1-精确查询，0-模糊查询，精确查询时device必须为设备ID";
                        tempJsonStr = JsonConvert.SerializeObject(joCS, jsonSetting);
                        break;
                    case JsonMethodType.BatAddDeviceFromIP:
                        JsonObjBatAddDeviceFromIP joBADFI = new JsonObjBatAddDeviceFromIP();
                        joBADFI.array = new List<arrayIP>();
                        joBADFI.array.Add(new arrayIP() { ip = "IP地址" });
                        joBADFI.array.Add(new arrayIP() { ip = "IP地址" });
                        joBADFI.devicetype = "设备类型";
                        joBADFI.addmode = "批量添加类型";
                        joBADFI.loginid = "设备连接名";
                        joBADFI.loginpwd = "设备连接密码";
                        joBADFI.rtspflag = "录像使用主辅流标记";
                        joBADFI.allurl = "使用主辅流地址是否是全地址";
                        joBADFI.mainurl = "主流地址";
                        joBADFI.auxurl = "辅流地址";
                        joBADFI.mic = "是否连接拾音器1-连接、0-未连接";
                        joBADFI.radio = "是否连接扬声器1-连接、0-未连接";
                        joBADFI.method = type;
                        tempJsonStr = JsonConvert.SerializeObject(joBADFI, jsonSetting);
                        break;
                    case JsonMethodType.BatAddDeviceFromXml:
                        JsonObjBatAddDeviceFromXml joBPDFX = new JsonObjBatAddDeviceFromXml();
                        arrayXml aX = new arrayXml();
                        aX.devicetype = "设备类型";
                        aX.devicename = "设备名";
                        aX.loginid = "设备连接名";
                        aX.loginpwd = "设备连接密码";
                        aX.flag = "录像使用主辅流标记";
                        aX.mainrtsp = "主流地址";
                        aX.auxrtsp = "辅流地址";
                        aX.devicestate = "设备状态";
                        aX.note = "备注";
                        aX.mic = "拾音器";
                        aX.radio = "扬声器";
                        joBPDFX.array.Add(aX);
                        joBPDFX.addmode = "批量添加类型，0-onvif扫描添加、1-手动ip段添加、2-xml文件导入添加";
                        joBPDFX.method = type;
                        //jsonSetting.TypeNameHandling = TypeNameHandling.Auto;
                        tempJsonStr = JsonConvert.SerializeObject(joBPDFX, jsonSetting);
                        break;
                    case JsonMethodType.BatAddDeviceImport:
                        return "http://[{0}]:[{1}]/xml/upload";
                    case JsonMethodType.BatPingDevice:
                        JsonObjBatPingDevice joBPD = new JsonObjBatPingDevice();
                        joBPD.method = type;
                        joBPD.firstIP = "开始IP";
                        joBPD.lastIP = "结束IP";
                        tempJsonStr = JsonConvert.SerializeObject(joBPD, jsonSetting);
                        break;
                    case JsonMethodType.AlarmSupplier:
                    case JsonMethodType.BASupplier:
                    case JsonMethodType.CVRSupplier:
                    case JsonMethodType.DecoderSupplier:
                    case JsonMethodType.DoorCardSupplier:
                    case JsonMethodType.NetKeySupplier:
                    case JsonMethodType.PSSupplier:
                        JsonObjDeviceId joDI = new JsonObjDeviceId();
                        joDI.method = type;
                        joDI.deviceid = "设备ID";
                        tempJsonStr = JsonConvert.SerializeObject(joDI, jsonSetting);
                        break;
                    case JsonMethodType.BindAlarmCamera:
                        JsonObjBindAlarmCamera joBAC = new JsonObjBindAlarmCamera();
                        joBAC.method = type;
                        joBAC.alarmid = "报警主机ID";
                        joBAC.areaid = "房间ID";
                        joBAC.cameraid = "摄像头ID";
                        joBAC.alarm_in_channel = "报警主机输入口";
                        joBAC.alarm_out_channel = "报警主机输出口";
                        tempJsonStr = JsonConvert.SerializeObject(joBAC, jsonSetting);
                        break;
                    case JsonMethodType.RebindAlarmCamera:
                        JsonObjRebindAlarmCamera joRBAC = new JsonObjRebindAlarmCamera();
                        joRBAC.method = type;
                        joRBAC.alarmid = "报警主机ID";
                        joRBAC.cameraid = "摄像头ID";
                        joRBAC.alarm_in_channel = "报警主机输入口";
                        joRBAC.alarm_out_channel = "报警主机输出口";
                        tempJsonStr = JsonConvert.SerializeObject(joRBAC, jsonSetting);
                        break;
                    case JsonMethodType.GetAlarmCamera:
                        JsonObjGetAlarmCamera joGAC = new JsonObjGetAlarmCamera();
                        joGAC.method = type;
                        joGAC.alarmid = "报警主机ID，查询时可为空";
                        joGAC.areaid = "房间ID，查询时可为空";
                        joGAC.cameraid = "摄像头ID，查询时可为空";
                        joGAC.alarm_in_channel = "报警主机输入口，查询时可为空";
                        joGAC.alarm_out_channel = "报警主机输出口，查询时可为空";
                        tempJsonStr = JsonConvert.SerializeObject(joGAC, jsonSetting);
                        break;
                    case JsonMethodType.BindBACamera:
                        JsonObjBindBACamera joBBAS = new JsonObjBindBACamera();
                        joBBAS.method = type;
                        joBBAS.deviceid = "设备ID";
                        joBBAS.cameraid = "摄像头ID";
                        joBBAS.channel = "通道号";
                        tempJsonStr = JsonConvert.SerializeObject(joBBAS, jsonSetting);
                        break;
                    case JsonMethodType.GetBACamera:
                    case JsonMethodType.DeleteBACamera:
                    case JsonMethodType.GetCVRCamera:
                        JsonObjGetBACamera joGBAC = new JsonObjGetBACamera();
                        joGBAC.method = type;
                        joGBAC.deviceid = "设备ID";
                        joGBAC.cameraid = "摄像头ID";
                        tempJsonStr = JsonConvert.SerializeObject(joGBAC, jsonSetting);
                        break;
                    case JsonMethodType.OpenBACamera:
                        JsonObjOpenBACamera joOBAC = new JsonObjOpenBACamera();
                        joOBAC.method = type;
                        joOBAC.deviceid = "分析仪设备ID";
                        joOBAC.cameraid = "摄像头ID";
                        joOBAC.channel = "通道号";
                        joOBAC.state = "设置状态1-启用，0-关闭";
                        tempJsonStr = JsonConvert.SerializeObject(joOBAC, jsonSetting);
                        break;
                    case JsonMethodType.BindCVRCamera:
                        JsonObjBindCVRCamera joBCVRC = new JsonObjBindCVRCamera();
                        joBCVRC.method = type;
                        joBCVRC.deviceid = "CVR ID";
                        joBCVRC.cameraid = "摄像头ID";
                        joBCVRC.channel = "通道号";
                        tempJsonStr = JsonConvert.SerializeObject(joBCVRC, jsonSetting);
                        break;
                    case JsonMethodType.DeleteCVRCamera:
                        JsonObjDeleteCVRCamera joDCVRC = new JsonObjDeleteCVRCamera();
                        joDCVRC.method = type;
                        joDCVRC.deviceid = "CVR ID";
                        joDCVRC.cameraid = "摄像头ID";
                        tempJsonStr = JsonConvert.SerializeObject(joDCVRC, jsonSetting);
                        break;
                    case JsonMethodType.GetCVRFileList:
                        JsonObjGetCVRFileList joGCVRFL = new JsonObjGetCVRFileList();
                        joGCVRFL.method = type;
                        joGCVRFL.deviceid = "存储设备ID，可为空";
                        joGCVRFL.cameraid = "摄像头ID";
                        joGCVRFL.starttime = "开始时间，格式(yyyy-MM-dd HH:MM:SS)";
                        joGCVRFL.endtime = "结束时间，格式(yyyy-MM-dd HH:MM:SS)";
                        tempJsonStr = JsonConvert.SerializeObject(joGCVRFL, jsonSetting);
                        break;
                    case JsonMethodType.StartDownloadFile:
                        JsonObjStartDownloadFile joSDFile = new JsonObjStartDownloadFile();
                        joSDFile.method = type;
                        joSDFile.deviceid = "存储设备ID，可为空";
                        joSDFile.cameraid = "摄像头ID";
                        joSDFile.starttime = "开始时间，格式(yyyy-MM-dd HH:MM:SS)";
                        joSDFile.endtime = "结束时间，格式(yyyy-MM-dd HH:MM:SS)";
                        joSDFile.savepath = "ftp保存路径，格式(/.../.../)";
                        joSDFile.savename = "录像保存文件名，不含后缀";
                        tempJsonStr = JsonConvert.SerializeObject(joSDFile, jsonSetting);
                        break;
                    case JsonMethodType.GetDownloadPos:
                        JsonObjGetDownloadPos joGDP = new JsonObjGetDownloadPos();
                        joGDP.method = type;
                        joGDP.deviceid = "CVR ID，可为空";
                        joGDP.cameraid = "摄像头ID";
                        joGDP.downhandle = "下载标记";
                        tempJsonStr = JsonConvert.SerializeObject(joGDP, jsonSetting);
                        break;
                    case JsonMethodType.StopDownload:
                        JsonObjStopDownload joSD = new JsonObjStopDownload();
                        joSD.method = type;
                        joSD.deviceid = "存储设备 ID，可为空";
                        joSD.cameraid = "摄像头ID";
                        joSD.downhandle = "下载标记,通过StartDownloadFile返回";
                        tempJsonStr = JsonConvert.SerializeObject(joSD, jsonSetting);
                        break;
                    case JsonMethodType.StartDownloadAreaFile:
                        JsonObjStartDownloadAreaFile joSDAFile = new JsonObjStartDownloadAreaFile();
                        joSDAFile.method = type;
                        joSDAFile.areaid = "房间ID";
                        joSDAFile.starttime = "开始时间，格式(yyyy-MM-dd HH:MM:SS)";
                        joSDAFile.endtime = "结束时间，格式(yyyy-MM-dd HH:MM:SS)";
                        joSDAFile.savepath = "ftp保存路径，格式(/.../.../)";
                        tempJsonStr = JsonConvert.SerializeObject(joSDAFile, jsonSetting);
                        break;
                    case JsonMethodType.PTZControl:
                        JsonObjPTZControl joPTZC = new JsonObjPTZControl();
                        joPTZC.method = type;
                        joPTZC.deviceid = "存储设备ID";
                        joPTZC.cameraid = "摄像头ID";
                        joPTZC.cmd = "命令";
                        joPTZC.speed = "速度(1-7),其他值，默认最快速度";
                        joPTZC.flag = "开始关闭标记，0-开始、1-停止。";
                        tempJsonStr = JsonConvert.SerializeObject(joPTZC, jsonSetting);
                        break;
                    case JsonMethodType.PTZPreset:
                        JsonObjPTZPreset joPTZP = new JsonObjPTZPreset();
                        joPTZP.method = type;
                        joPTZP.deviceid = "存储设备ID，可为空";
                        joPTZP.cameraid = "摄像头ID";
                        joPTZP.cmd = "命令，8-设置预置点、9-清除预置点、39-转到预置点";
                        joPTZP.index = "预置点序号1-300";
                        tempJsonStr = JsonConvert.SerializeObject(joPTZP, jsonSetting);
                        break;
                    case JsonMethodType.GetDoorCardLog:
                        JsonObjGetDoorCardLog joGDCL = new JsonObjGetDoorCardLog();
                        joGDCL.method = type;
                        joGDCL.cardid = "门卡ID,可以为空";
                        joGDCL.begtime = "开始时间（格式：yyyy-MM-dd）,可以为空";
                        joGDCL.endtime = "结束时间（格式：yyyy-MM-dd）,可以为空";
                        joGDCL.user = "刷卡用户名或ID,可以为空";
                        joGDCL.area = "房间名或ID,可以为空";
                        joGDCL.doorcard = "门禁主机名或ID,可以为空";
                        joGDCL.cardreader = "门禁读卡器名或ID,可以为空";
                        tempJsonStr = JsonConvert.SerializeObject(joGDCL, jsonSetting);
                        break;
                    case JsonMethodType.GetSequencerConf:
                        JsonObjGetSequencerConf joGSC = new JsonObjGetSequencerConf();
                        joGSC.method = type;
                        joGSC.area = "房间名或ID,可以为空";
                        joGSC.sequencer = "电源时序器名或ID,可以为空";
                        tempJsonStr = JsonConvert.SerializeObject(joGSC, jsonSetting);
                        break;
                    case JsonMethodType.SequencerConf:
                        JsonObjSequencerConf joSC = new JsonObjSequencerConf();
                        joSC.method = type;
                        joSC.areaid = "房间ID";
                        joSC.sequencerid = "电源时序器ID";
                        joSC.bsotype = "时序器业务模块类型";
                        joSC.openorder = "启动顺序，00000001，字符串长度和电源时序器的插口数量一致，1-开启，0-不开启";
                        joSC.closeorder = "关闭顺序，00000001，字符串长度和电源时序器的插口数量一致，1-关闭，0-不关闭";
                        tempJsonStr = JsonConvert.SerializeObject(joSC, jsonSetting);
                        break;
                    case JsonMethodType.DeleteSequencerConf:
                        JsonObjDeleteSequencerConf joDSC = new JsonObjDeleteSequencerConf();
                        joDSC.method = type;
                        joDSC.areaid = "房间ID";
                        joDSC.sequencerid = "电源时序器ID";
                        joDSC.bsotype = "业务模块类型";
                        tempJsonStr = JsonConvert.SerializeObject(joDSC, jsonSetting);
                        break;
                    case JsonMethodType.SyncRecordSupplier:
                        JsonObjSyncRecordSupplier joSRS = new JsonObjSyncRecordSupplier();
                        joSRS.method = type;
                        joSRS.deviceid = "设备ID";
                        tempJsonStr = JsonConvert.SerializeObject(joSRS, jsonSetting);
                        break;
                    case JsonMethodType.BindSyncRecordCamera:
                        JsonObjBindSyncRecordCamera joBSRC = new JsonObjBindSyncRecordCamera();
                        joBSRC.method = type;
                        joBSRC.deviceid = "同录设备ID";
                        joBSRC.cameraid = "摄像头ID";
                        joBSRC.channel = "通道号";
                        tempJsonStr = JsonConvert.SerializeObject(joBSRC, jsonSetting);
                        break;
                    case JsonMethodType.GetSyncRecordCamera:
                        JsonObjGetSyncRecordCamera joGSRC = new JsonObjGetSyncRecordCamera();
                        joGSRC.method = type;
                        joGSRC.deviceid = "同录设备ID";
                        joGSRC.cameraid = "摄像头ID";
                        tempJsonStr = JsonConvert.SerializeObject(joGSRC, jsonSetting);
                        break;
                    case JsonMethodType.DeleteSyncRecordCamera:
                        JsonObjDeleteSyncRecordCamera joDSRC = new JsonObjDeleteSyncRecordCamera();
                        joDSRC.method = type;
                        joDSRC.deviceid = "同录设备ID";
                        joDSRC.cameraid = "摄像头ID";
                        tempJsonStr = JsonConvert.SerializeObject(joDSRC, jsonSetting);
                        break;
                    case JsonMethodType.AddArea:
                        JsonObjAddArea joAA = new JsonObjAddArea();
                        joAA.method = type;
                        joAA.areaname = "房间名";
                        joAA.areatype = "房间类型";
                        joAA.note = "备注";
                        tempJsonStr = JsonConvert.SerializeObject(joAA, jsonSetting);
                        break;
                    case JsonMethodType.DelArea:
                        JsonObjDelArea joDA = new JsonObjDelArea();
                        joDA.method = type;
                        joDA.areaid = "房间ID";
                        tempJsonStr = JsonConvert.SerializeObject(joDA, jsonSetting);
                        break;
                    case JsonMethodType.UpdateArea:
                        JsonObjUpdateArea joUA = new JsonObjUpdateArea();
                        joUA.method = type;
                        joUA.areaid = "房间ID";
                        joUA.areaname = "房间名";
                        joUA.areatype = "房间类型";
                        joUA.note = "备注";
                        tempJsonStr = JsonConvert.SerializeObject(joUA, jsonSetting);
                        break;
                    case JsonMethodType.GetArea:
                        JsonObjGetArea joGA = new JsonObjGetArea();
                        joGA.method = type;
                        joGA.area = "房间ID或房间名";
                        joGA.isexact = "精确查找标记";
                        tempJsonStr = JsonConvert.SerializeObject(joGA, jsonSetting);
                        break;
                    case JsonMethodType.BindAreaDevice:
                        JsonObjBindAreaDevice joBAD = new JsonObjBindAreaDevice();
                        joBAD.method = type;
                        joBAD.areaid = "房间ID";
                        arrayArea area = new arrayArea();
                        area.deviceid = "设备ID";
                        area.devicename = "设备名";
                        area.devicetype = "设备类型";
                        joBAD.array.Add(area);
                        joBAD.array.Add(area);
                        tempJsonStr = JsonConvert.SerializeObject(joBAD, jsonSetting);
                        break;
                    case JsonMethodType.GetAreaDevice:
                        JsonObjGetAreaDevice joGAD = new JsonObjGetAreaDevice();
                        joGAD.method = type;
                        joGAD.areaid = "房间ID";
                        tempJsonStr = JsonConvert.SerializeObject(joGAD, jsonSetting);
                        break;
                    case JsonMethodType.GetAreaCamera:
                        JsonObjGetAreaCamera joGetAC = new JsonObjGetAreaCamera();
                        joGetAC.method = type;
                        joGetAC.areaid = "房间ID,此属性不空时为精确查询";
                        joGetAC.areaname = "房间ID或房间名模糊查询";
                        tempJsonStr = JsonConvert.SerializeObject(joGetAC, jsonSetting);
                        break;
                    case JsonMethodType.AddUser:
                        JsonObjAddUser joAU = new JsonObjAddUser();
                        joAU.method = type;
                        joAU.userid = "用户ID唯一，如果当期系统没有该用户，默认按照添加处理";
                        joAU.pwd = "用户登录密码，MD5加密后的";
                        joAU.username = "真实姓名";
                        joAU.telephone = "办公室电话";
                        joAU.mobile = "移动电话";
                        joAU.email = "电子邮件";
                        arrayUser userAU = new arrayUser();
                        userAU.roleid = "角色ID";
                        joAU.array.Add(userAU);
                        joAU.array.Add(userAU);
                        tempJsonStr = JsonConvert.SerializeObject(joAU, jsonSetting);
                        break;
                    case JsonMethodType.UpdateUser:
                        JsonObjUpdateUser joUU = new JsonObjUpdateUser();
                        joUU.method = type;
                        joUU.userid = "用户ID唯一，如果当期系统没有该用户，默认按照添加处理";
                        joUU.username = "真实姓名";
                        joUU.telephone = "办公室电话";
                        joUU.mobile = "移动电话";
                        joUU.email = "电子邮件";
                        arrayUser userUU = new arrayUser();
                        userUU.roleid = "角色ID";
                        joUU.array.Add(userUU);
                        joUU.array.Add(userUU);
                        tempJsonStr = JsonConvert.SerializeObject(joUU, jsonSetting);
                        break;
                    case JsonMethodType.UpdateUserInfo:
                        JsonObjUpdateUserInfo joUUI = new JsonObjUpdateUserInfo();
                        joUUI.method = type;
                        joUUI.userid = "用户ID";
                        joUUI.pwd = "用户登录密码，MD5加密后的";
                        joUUI.username = "真实姓名";
                        joUUI.telephone = "办公室电话";
                        joUUI.mobile = "移动电话";
                        joUUI.email = "电子邮件";
                        tempJsonStr = JsonConvert.SerializeObject(joUUI, jsonSetting);
                        break;
                    case JsonMethodType.DeleteUser:
                        JsonObjDeleteUser joDU = new JsonObjDeleteUser();
                        joDU.method = type;
                        joDU.userid = "用户ID唯一，如果当期系统没有该用户，默认按照添加处理";
                        tempJsonStr = JsonConvert.SerializeObject(joDU, jsonSetting);
                        break;
                    case JsonMethodType.UserLogin:
                        JsonObjUserLogin joUL = new JsonObjUserLogin();
                        joUL.method = type;
                        joUL.userid = "用户ID唯一，如果当期系统没有该用户，默认按照添加处理";
                        joUL.pwd = "用户登录密码，MD5加密后的";
                        joUL.flag = "0-默认登录查询所有权限，1-winform客户端登录";
                        tempJsonStr = JsonConvert.SerializeObject(joUL, jsonSetting);
                        break;
                    case JsonMethodType.UserSearch:
                    case JsonMethodType.CheckUserID:
                    case JsonMethodType.ResetUserPwd:
                        JsonObjUserSearch joUS = new JsonObjUserSearch();
                        joUS.method = type;
                        joUS.userid = "用户ID";
                        tempJsonStr = JsonConvert.SerializeObject(joUS, jsonSetting);
                        break;
                    case JsonMethodType.GetUser:
                        JsonObjGetUser joGU = new JsonObjGetUser();
                        joGU.method = type;
                        joGU.user = "用户ID、用户名、角色名";
                        joGU.flag = "1-winform,0或空查询所有权限";
                        joGU.isexact = "1-精确查询，0-模糊查询";
                        tempJsonStr = JsonConvert.SerializeObject(joGU, jsonSetting);
                        break;
                    case JsonMethodType.UpdatePwd:
                        JsonObjUpdatePwd joUP = new JsonObjUpdatePwd();
                        joUP.method = type;
                        joUP.userid = "用户ID";
                        joUP.oldpwd = "旧密码";
                        joUP.pwd = "新密码";
                        tempJsonStr = JsonConvert.SerializeObject(joUP, jsonSetting);
                        break;
                    case JsonMethodType.BindUserRole:
                        JsonObjBindUserRole joBUR = new JsonObjBindUserRole();
                        joBUR.method = type;
                        joBUR.userid = "需要绑定的用户ID";
                        arrayUser userBUR = new arrayUser();
                        userBUR.roleid = "角色ID";
                        joBUR.array.Add(userBUR);
                        joBUR.array.Add(userBUR);
                        tempJsonStr = JsonConvert.SerializeObject(joBUR, jsonSetting);
                        break;

                    case JsonMethodType.AddPrivilege:
                        JsonObjAddPrivilege joAP = new JsonObjAddPrivilege();
                        joAP.method = type;
                        joAP.privilegename = "权限名(唯一)";
                        arrayPrivilege arrAddPrivi = new arrayPrivilege();
                        arrAddPrivi.deviceid = "设备ID";
                        arrAddPrivi.devicename = "设备名";
                        joAP.array.Add(arrAddPrivi);
                        tempJsonStr = JsonConvert.SerializeObject(joAP, jsonSetting);
                        break;
                    case JsonMethodType.UpdatePrivilege:
                        JsonObjUpdatePrivilege joUpP = new JsonObjUpdatePrivilege();
                        joUpP.method = type;
                        joUpP.privilegeid = "权限ID";
                        joUpP.privilegename = "权限名，权限名可以是现有权限，也可以是当前系统没有的权限，没有时默认按照新加权限处理";
                        arrayPrivilege arrUpdatePrivi = new arrayPrivilege();
                        arrUpdatePrivi.deviceid = "设备ID";
                        arrUpdatePrivi.devicename = "设备名";
                        joUpP.array.Add(arrUpdatePrivi);
                        tempJsonStr = JsonConvert.SerializeObject(joUpP, jsonSetting);
                        break;
                    case JsonMethodType.DeletePrivilege:
                        JsonObjDeletePrivilege joDelP = new JsonObjDeletePrivilege();
                        joDelP.method = type;
                        joDelP.privilegeid = "权限ID";
                        joDelP.privilegename = "权限名";
                        tempJsonStr = JsonConvert.SerializeObject(joDelP, jsonSetting);
                        break;
                    case JsonMethodType.RolePrivilege:
                        JsonObjRolePrivilege joRP = new JsonObjRolePrivilege();
                        joRP.method = type;
                        joRP.roleid = "角色ID";
                        tempJsonStr = JsonConvert.SerializeObject(joRP, jsonSetting);
                        break;
                    case JsonMethodType.PrivilegeDevice:
                        JsonObjPrivilegeDevice joPD = new JsonObjPrivilegeDevice();
                        joPD.method = type;
                        joPD.privilegeid = "权限ID";
                        tempJsonStr = JsonConvert.SerializeObject(joPD, jsonSetting);
                        break;
                    case JsonMethodType.GetPrivilege:
                        JsonObjGetPrivilege joGP = new JsonObjGetPrivilege();
                        joGP.method = type;
                        joGP.flag = "0-返回全部，1-winform客户端权限";
                        joGP.privilege = "权限ID/权限名/设备名";
                        joGP.isexact = "是否精确查询";
                        tempJsonStr = JsonConvert.SerializeObject(joGP, jsonSetting);
                        break;
                    case JsonMethodType.AddRole:
                        JsonObjAddRole joAR = new JsonObjAddRole();
                        joAR.method = type;
                        joAR.rolename = "角色名";
                        arrayRole arrAddRole = new arrayRole();
                        arrAddRole.privilegeid = "权限ID";
                        arrAddRole.privilegename = "权限名";
                        joAR.array.Add(arrAddRole);
                        tempJsonStr = JsonConvert.SerializeObject(joAR, jsonSetting);
                        break;
                    case JsonMethodType.UpdateRole:
                        JsonObjUpdateRole joUR = new JsonObjUpdateRole();
                        joUR.method = type;
                        joUR.rolename = "角色名，角色可以是现有角色，也可以是当前系统没有的角色，没有时默认按照新加角色处理";
                        joUR.roleid = "角色ID";
                        arrayRole arrRole = new arrayRole();
                        arrRole.privilegeid = "权限ID";
                        arrRole.privilegename = "权限名";
                        joUR.array.Add(arrRole);
                        tempJsonStr = JsonConvert.SerializeObject(joUR, jsonSetting);
                        break;
                    case JsonMethodType.DeleteRole:
                        JsonObjDeleteRole joDR = new JsonObjDeleteRole();
                        joDR.method = type;
                        joDR.rolename = "角色名";
                        joDR.roleid = "角色ID";
                        tempJsonStr = JsonConvert.SerializeObject(joDR, jsonSetting);
                        break;
                    case JsonMethodType.UserRole:
                        JsonObjUserRole joUserR = new JsonObjUserRole();
                        joUserR.method = type;
                        joUserR.userid = "用户登录ID";
                        tempJsonStr = JsonConvert.SerializeObject(joUserR, jsonSetting);
                        break;
                    case JsonMethodType.GetRole:
                        JsonObjGetRole joGetR = new JsonObjGetRole();
                        joGetR.method = type;
                        joGetR.role = "角色ID/角色名/权限名，等于空字符串时查询所有角色";
                        joGetR.isexact = "1-精确查询，0-模糊查询";
                        tempJsonStr = JsonConvert.SerializeObject(joGetR, jsonSetting);
                        break;
                    case JsonMethodType.AddPlat:
                        JsonObjAddPlat joAddP = new JsonObjAddPlat();
                        joAddP.method = type;
                        joAddP.port = "远程平台端口";
                        joAddP.remoteip = "远程子平台IP";
                        tempJsonStr = JsonConvert.SerializeObject(joAddP, jsonSetting);
                        break;
                    case JsonMethodType.UpdatePlat:
                        JsonObjUpdatePlat joUPlat = new JsonObjUpdatePlat();
                        joUPlat.method = type;
                        joUPlat.ip = "新IP地址";
                        joUPlat.port = "新端口";
                        joUPlat.platid = "子平台ID";
                        tempJsonStr = JsonConvert.SerializeObject(joUPlat, jsonSetting);
                        break;
                    case JsonMethodType.DeletePlat:
                        JsonObjDeletePlat joDPlat = new JsonObjDeletePlat();
                        joDPlat.method = type;
                        joDPlat.platid = "子平台ID";
                        tempJsonStr = JsonConvert.SerializeObject(joDPlat, jsonSetting);
                        break;
                    case JsonMethodType.PlatListExact:
                        JsonObjPlatListExact joPL = new JsonObjPlatListExact();
                        joPL.method = type;
                        joPL.plat = "";
                        joPL.isexact = "";
                        tempJsonStr = JsonConvert.SerializeObject(joPL, jsonSetting);
                        break;
                    case JsonMethodType.AddGroup:
                        JsonObjAddGroup joAG = new JsonObjAddGroup();
                        joAG.method = type;
                        joAG.groupname = "组名当前系统唯一";
                        joAG.parentgroupid = "上级组ID，没有上级组此属性为空或不填写此属性";
                        tempJsonStr = JsonConvert.SerializeObject(joAG, jsonSetting);
                        break;
                    case JsonMethodType.UpdateGroup:
                        JsonObjUpdateGroup joUG = new JsonObjUpdateGroup();
                        joUG.method = type;
                        joUG.groupname = "组名当前系统唯一";
                        joUG.parentgroupid = "上级组ID，没有上级组此属性为空或不填写此属性";
                        joUG.groupid = "组ID";
                        tempJsonStr = JsonConvert.SerializeObject(joUG, jsonSetting);
                        break;
                    case JsonMethodType.DeleteGroup:
                        JsonObjDeleteGroup joDG = new JsonObjDeleteGroup();
                        joDG.method = type;
                        joDG.groupid = "组ID";
                        tempJsonStr = JsonConvert.SerializeObject(joDG, jsonSetting);
                        break;
                    case JsonMethodType.PlatGroup:
                        JsonObjPlatGroup joPG = new JsonObjPlatGroup();
                        joPG.method = type;
                        joPG.platid = "平台ID，如果是当前平台不需要此属性";
                        tempJsonStr = JsonConvert.SerializeObject(joPG, jsonSetting);
                        break;
                    case JsonMethodType.GroupSearch:
                        JsonObjGroupSearch joGS = new JsonObjGroupSearch();
                        joGS.method = type;
                        joGS.group = "组ID或组名";
                        joGS.isexact = "1-精确查询，0-模糊查询";
                        tempJsonStr = JsonConvert.SerializeObject(joGS, jsonSetting);
                        break;
                    case JsonMethodType.KeepMove:
                        JsonObjKeepMove joKM = new JsonObjKeepMove();
                        joKM.method = type;
                        joKM.deviceid = "设备ID";
                        joKM.x = "x轴移动速度";
                        joKM.y = "y轴移动速度";
                        joKM.z = "调整变焦，z有值时，x,y值默认无效，操作默认为调焦.";
                        tempJsonStr = JsonConvert.SerializeObject(joKM, jsonSetting);
                        break;
                    case JsonMethodType.StopMove:
                        JsonObjStopMove joSM = new JsonObjStopMove();
                        joSM.method = type;
                        joSM.deviceid = "设备ID";
                        joSM.xyz = "值是z时停止调焦，值是x,y时停止移动。";
                        tempJsonStr = JsonConvert.SerializeObject(joSM, jsonSetting);
                        break;
                    case JsonMethodType.RltMove:
                        JsonObjRltMove joRltM = new JsonObjRltMove();
                        joRltM.method = type;
                        joRltM.deviceid = "设备ID";
                        joRltM.x = "x轴移动速度";
                        joRltM.y = "y轴移动速度";
                        joRltM.z = "调整变焦，z有值时，x,y值默认无效，操作默认为调焦.";
                        tempJsonStr = JsonConvert.SerializeObject(joRltM, jsonSetting);
                        break;
                    case JsonMethodType.GetPresets:
                        JsonObjGetPresets joGetP = new JsonObjGetPresets();
                        joGetP.method = type;
                        joGetP.deviceid = "设备ID";
                        tempJsonStr = JsonConvert.SerializeObject(joGetP, jsonSetting);
                        break;
                    case JsonMethodType.SetPresets:
                    case JsonMethodType.RemovePresets:
                    case JsonMethodType.GotoPresets:
                        JsonObjSetPresets joSP = new JsonObjSetPresets();
                        joSP.method = type;
                        joSP.deviceid = "设备ID";
                        joSP.name = "预置点名";
                        joSP.Token = "预置点token，如果是新增预置点，此处设置””或”-1”";
                        tempJsonStr = JsonConvert.SerializeObject(joSP, jsonSetting);
                        break;
                    case JsonMethodType.RtspRelay:
                        JsonObjRtspRelay joRtspR = new JsonObjRtspRelay();
                        joRtspR.method = type;
                        joRtspR.deviceid = "(下级平台+)设备ID,多个设备ID以|分割，此处不支持下级平台设备的批量转发请求";
                        joRtspR.flag = "主辅流标记0-主流，1-辅流";
                        tempJsonStr = JsonConvert.SerializeObject(joRtspR, jsonSetting);
                        break;
                    case JsonMethodType.TSFileRelay:
                        JsonObjTSFileRelay joTSFR = new JsonObjTSFileRelay();
                        joTSFR.method = type;
                        joTSFR.deviceid = "(下级平台+)设备ID，如果是本级平台设备，此处是设备ID，如果是下级(可能多级)，规则是：子平台ID/././设备ID";
                        joTSFR.begtime = "开始时间";
                        joTSFR.endtime = "结束时间";
                        tempJsonStr = JsonConvert.SerializeObject(joTSFR, jsonSetting);
                        break;
                    case JsonMethodType.TSFileSeekPlay:
                        JsonObjTSFileSeekPlay joTSFSP = new JsonObjTSFileSeekPlay();
                        joTSFSP.method = type;
                        joTSFSP.deviceid = "(下级平台+)设备ID，如果是本级平台设备，此处是设备ID，如果是下级(可能多级)，规则是：子平台ID/././设备ID";
                        joTSFSP.videoname = "视频名，TSFileRelay请求返回";
                        joTSFSP.videopos = "播放时间点（进度）";
                        tempJsonStr = JsonConvert.SerializeObject(joTSFSP, jsonSetting);
                        break;
                    case JsonMethodType.DownloadFileList:
                        JsonObjDownloadFileList joDFL = new JsonObjDownloadFileList();
                        joDFL.method = type;
                        joDFL.deviceid = "设备ID";
                        joDFL.begtime = "开始时间";
                        joDFL.endtime = "结束时间";
                        tempJsonStr = JsonConvert.SerializeObject(joDFL, jsonSetting);
                        break;
                    case JsonMethodType.GetRelayRtsp:
                        JsonObjGetRelayRtsp joGRR = new JsonObjGetRelayRtsp();
                        joGRR.method = type;
                        joGRR.deviceid = "设备ID";
                        joGRR.audio = "是否带有音频，0-查询没有音频的视频流、1-查询有音频的视频流，此属性为1时检查有音频和无音频两种视频流，查到有音频的流直接返回，如果没有带有音频的流而有无音频视频流返回无音频视频流地址";
                        joGRR.flag = "主辅流标记0-主流、1-辅流";
                        tempJsonStr = JsonConvert.SerializeObject(joGRR, jsonSetting);
                        break;
                    case JsonMethodType.AddPlan:
                    case JsonMethodType.UpdatePlan:
                        JsonObjAddPlan joAPlan = new JsonObjAddPlan();
                        joAPlan.method = type;
                        joAPlan.deviceid = "设备ID";
                        joAPlan.begdate = "开始日期，格式：YYYY-MM-DD";
                        joAPlan.enddate = "结束日期，格式：YYYY-MM-DD";
                        joAPlan.begloop = "开始循环标记";
                        joAPlan.endloop = "结束循环标记";
                        joAPlan.begtime = "开始时间，格式：hh:mm:ss";
                        joAPlan.endtime = "结束时间，格式：hh:mm:ss";
                        joAPlan.loopflag = "循环标志 0-按天、1-按周、2-按月";
                        joAPlan.tasktype = "任务类型，0-录像，现在有录像";
                        joAPlan.userid = "创建者ID";
                        joAPlan.planname = "计划名";
                        tempJsonStr = JsonConvert.SerializeObject(joAPlan, jsonSetting);
                        break;
                    case JsonMethodType.PlanSearch:
                        JsonObjPlanSearch joPlanS = new JsonObjPlanSearch();
                        joPlanS.method = type;
                        joPlanS.plan = "查询条件，可以是计划名、设备名、设备ID";
                        tempJsonStr = JsonConvert.SerializeObject(joPlanS, jsonSetting);
                        break;
                    case JsonMethodType.ExactPlanSearch:
                    case JsonMethodType.DeletePlan:
                        JsonObjExactPlanSearch joEPS = new JsonObjExactPlanSearch();
                        joEPS.method = type;
                        joEPS.deviceid = "设备ID";
                        joEPS.planname = "计划名";
                        tempJsonStr = JsonConvert.SerializeObject(joEPS, jsonSetting);
                        break;
                    case JsonMethodType.ClosePlan:
                        JsonObjClosePlan joCP = new JsonObjClosePlan();
                        joCP.method = type;
                        joCP.deviceid = "设备ID";
                        joCP.planname = "计划名";
                        joCP.state = "状态，0-停用、1-启用";
                        tempJsonStr = JsonConvert.SerializeObject(joCP, jsonSetting);
                        break;
                    case JsonMethodType.BatAddPlan:
                        JsonObjBatAddPlan joBAP = new JsonObjBatAddPlan();
                        joBAP.method = type;
                        joBAP.method = type;
                        joBAP.deviceid = "设备ID";
                        joBAP.begdate = "开始日期，格式：YYYY-MM-DD";
                        joBAP.enddate = "结束日期，格式：YYYY-MM-DD";
                        joBAP.begloop = "开始循环标记，如果按天循环可以不填、按周循环是为0-6，星期日为0，星期六为6、按月循环时，具体日期12号即为12";
                        joBAP.endloop = "结束循环标记";
                        joBAP.begtime = "开始时间，格式：hh:mm:ss";
                        joBAP.endtime = "结束时间，格式：hh:mm:ss";
                        joBAP.loopflag = "循环标志 0-按天、1-按周、2-按月";
                        joBAP.tasktype = "任务类型，0-录像，现在有录像";
                        joBAP.userid = "创建者ID";
                        arrayPlan arrPlan = new arrayPlan();
                        arrPlan.planname = "计划名（与deviceid构成唯一约束）";
                        arrPlan.deviceid = "设备ID（与planname构成唯一约束）";
                        tempJsonStr = JsonConvert.SerializeObject(joBAP, jsonSetting);
                        break;
                    case JsonMethodType.MP4Preview:
                        JsonObjMP4Preview joMP4P = new JsonObjMP4Preview();
                        joMP4P.method = type;
                        joMP4P.loginid = "设备登录ID，可以为空，onvif扫描出的地址预览时不能为空";
                        joMP4P.loginpwd = "设备登录密码，可以为空，onvif扫描出的地址预览时不能为空";
                        joMP4P.url = "预览视频流的rtsp地址";
                        tempJsonStr = JsonConvert.SerializeObject(joMP4P, jsonSetting);
                        break;
                    case JsonMethodType.DevicePreview:
                        JsonObjDevicePreview joDPre = new JsonObjDevicePreview();
                        joDPre.method = type;
                        joDPre.deviceid = "设备ID";
                        joDPre.flag = "主辅流标记0-主流，1-辅流。";
                        tempJsonStr = JsonConvert.SerializeObject(joDPre, jsonSetting);
                        break;
                    case JsonMethodType.RMSConf:
                        JsonObjRMSConf joRMSC = new JsonObjRMSConf();
                        joRMSC.method = type;
                        joRMSC.ip = "测试数据库服务器IP";
                        joRMSC.port = "端口";
                        joRMSC.retain_time = "循环删除录像时间小时为单位，超过此时间的视频文件删除";
                        joRMSC.src_num = "最大接入设备数量，现在不限制，填写固定值9999";
                        joRMSC.reconn_time = "资源断线重连时间秒为单位";
                        joRMSC.save_type = "保存类型0-TS、1-MP4，现在只录制一种TS，填写固定值0";
                        joRMSC.length = "单媒体文件长度秒为单位";
                        joRMSC.section = "切片数量，一个m3u8文件对应多少ts文件";
                        joRMSC.save_pos = "保持位置，0-本地、1-云存储，现在只保存本地，填写固定值0";
                        joRMSC.local_path = "本地路径";
                        joRMSC.leave_space = "最大使用空间，当磁盘空间使用达到上限时执行over_opt的操作";
                        joRMSC.over_opt = "当磁盘空间使用达到上限时采取的操作";
                        tempJsonStr = JsonConvert.SerializeObject(joRMSC, jsonSetting);
                        break;
                    case JsonMethodType.DBConf:
                    case JsonMethodType.CheckDB:
                        JsonObjDBConf joDBConf = new JsonObjDBConf();
                        joDBConf.method = type;
                        joDBConf.ip = "测试数据库服务器IP";
                        joDBConf.port = "端口";
                        joDBConf.dbname = "测试数据库名";
                        joDBConf.dbuser = "用户名";
                        joDBConf.dbpwd = "密码";
                        tempJsonStr = JsonConvert.SerializeObject(joDBConf, jsonSetting);
                        break;
                    case JsonMethodType.ReStart:
                        JsonObjReStart joReStart = new JsonObjReStart();
                        joReStart.method = type;
                        joReStart.server = "重启的服务器，ALL/CMS/HDR/RMS, ALL-重启所有服务、CMS-CMS服务器、HDR-转发服务器、RMS-录像服务器";
                        tempJsonStr = JsonConvert.SerializeObject(joReStart, jsonSetting);
                        break;
                    case JsonMethodType.AllDataSync:
                        tempJsonStr = CreateJsonObjAllDataSync(type, jsonSetting);
                        break;
                    case JsonMethodType.DataSync:
                        tempJsonStr = CreateJsonObjDataSync(type, jsonSetting);
                        break;
                    default:
                        break;
                }
                //tempJsonStr = JsonConvert.SerializeObject(tempObj, jsonSetting);
                temp = ConvertJsonString(tempJsonStr);
                return temp;
            }
            catch (System.Exception ex)
            {
                return string.Format("An error occurred ", ex.Message);
            }
        }

        /// <summary>
        /// 通过对应的Json对象返回对应的Json String,内容为Request样例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected internal string JsonSampleStringCreator(string type)
        {
            string temp = string.Empty;
            try
            {
                //通过TreeView节点的Name，获取对应的枚举值
                JsonMethodType jsonMethodType = (JsonMethodType)Enum.Parse(typeof(JsonMethodType), type);
                JsonSerializerSettings jsonSetting = new JsonSerializerSettings();
                //解析时忽略Null Value的属性
                jsonSetting.NullValueHandling = NullValueHandling.Ignore;
                //可以解析继承类
                jsonSetting.TypeNameHandling = TypeNameHandling.Auto;
                jsonSetting.ConstructorHandling = ConstructorHandling.Default;
                jsonSetting.Formatting = Formatting.None;
                jsonSetting.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //object tempObj= null;
                string tempJsonStr = string.Empty;
                switch (jsonMethodType)
                {
                    case JsonMethodType.Version:
                    case JsonMethodType.RunStatus:
                    case JsonMethodType.SysInfo:
                    case JsonMethodType.PlatList:
                    case JsonMethodType.PlatInfo:
                    case JsonMethodType.RmsFtpInfo:
                    case JsonMethodType.AllPlanSearch:
                    case JsonMethodType.StopPreview:
                    case JsonMethodType.GetRMSConf:
                    case JsonMethodType.GetDBConf:
                    case JsonMethodType.ReqDataSync:
                    case JsonMethodType.SaveDevFile:
                        JsonObjVersion joRS = new JsonObjVersion();
                        joRS.method = type;
                        tempJsonStr = JsonConvert.SerializeObject(joRS, jsonSetting);
                        break;
                    case JsonMethodType.UserLogSearch:
                        JsonObjUserLogSearch jrULS = new JsonObjUserLogSearch();
                        jrULS.method = type;
                        jrULS.begtime = "2017-11-11";
                        jrULS.endtime = "2017-12-12";
                        jrULS.user = "";
                        tempJsonStr = JsonConvert.SerializeObject(jrULS, jsonSetting);
                        break;
                    case JsonMethodType.AddDevice:
                        JsonObjAddDevice joAD = new JsonObjAddDevice();
                        joAD.method = type;
                        joAD.devicetype = "50";// 50为报警主机
                        joAD.name = "报警主机31";
                        joAD.ip = "127.0.0.1";
                        joAD.port = "555";
                        joAD.user = "admin";
                        joAD.pwd = "admin@123";
                        joAD.pin = "DS-19A08-01BNE";
                        joAD.supplier = "1";
                        joAD.in_channel = "256";
                        joAD.out_channel = "1";
                        joAD.in_out = "1";
                        joAD.chipin_count = "8";
                        joAD.channel_count = "48";
                        tempJsonStr = JsonConvert.SerializeObject(joAD, jsonSetting);
                        break;
                    case JsonMethodType.AddCamera:
                        JsonObjAddCamera joAC = new JsonObjAddCamera();
                        joAC.method = type;
                        joAC.devicetype = "2";
                        joAC.devicename = "设备名1";
                        joAC.bindid = "";
                        joAC.groupid = "";
                        joAC.loginid = "admin";
                        joAC.loginpwd = "12345qwert";
                        joAC.ip = "10.10.1.200";
                        joAC.port = "554";
                        joAC.mainrtsp = "rtsp://admin:12345qwert@10.10.1.200/Streaming/Channels/101";
                        joAC.auxrtsp = "rtsp://admin:12345qwert@10.10.1.200/Streaming/Channels/102";
                        joAC.flag = "0";
                        joAC.devicestate = "可用";
                        joAC.note = "备注";
                        joAC.mic = "1";
                        joAC.radio = "0";
                        tempJsonStr = JsonConvert.SerializeObject(joAC, jsonSetting);
                        break;
                    case JsonMethodType.DelDevice:
                        JsonObjDelDevice joDD = new JsonObjDelDevice();
                        joDD.method = type;
                        joDD.deviceid = "CMS9999-D2000001";
                        tempJsonStr = JsonConvert.SerializeObject(joDD, jsonSetting);
                        break;
                    case JsonMethodType.DeleteCamera:
                        JsonObjDeleteCamera joDC = new JsonObjDeleteCamera();
                        joDC.method = type;
                        joDC.deviceid = "CMS9999-D2000001";
                        joDC.devicename = "设备名4";
                        joDC.delrecord = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joDC, jsonSetting);
                        break;
                    case JsonMethodType.UpdateDevice:
                        JsonObjUpdateDevice joUD = new JsonObjUpdateDevice();
                        joUD.method = type;
                        joUD.deviceid = "CMS9999-D5000001";
                        joUD.devicetype = "50";
                        joUD.name = "报警主机02";
                        joUD.ip = "10.10.1.110";
                        joUD.port = "555";
                        joUD.user = "admin";
                        joUD.pwd = "12345";
                        joUD.in_channel = "256";
                        joUD.out_channel = "1";
                        joUD.chipin_count = "";
                        joUD.channel_count = "";
                        tempJsonStr = JsonConvert.SerializeObject(joUD, jsonSetting);
                        break;
                    case JsonMethodType.UpdateCamera:
                        JsonObjUpdateCamera joUC = new JsonObjUpdateCamera();
                        joUC.method = type;
                        joUC.deviceid = "CMS0001-D2000001";
                        joUC.devicetype = "2";
                        joUC.devicename = "设备002";
                        joUC.bindid = "";
                        joUC.groupid = "";
                        joUC.loginid = "admin";
                        joUC.loginpwd = "12345qwert";
                        joUC.ip = "10.10.1.200";
                        joUC.port = "553";
                        joUC.mainrtsp = "rtsp://admin:12345qwert@10.10.1.200/Streaming/Channels/101";
                        joUC.auxrtsp = "rtsp://admin:12345qwert@10.10.1.200/Streaming/Channels/102";
                        joUC.flag = "0";
                        joUC.devicestate = "可用";
                        joUC.note = "备注";
                        joUC.mic = "0";
                        joUC.radio = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joUC, jsonSetting);
                        break;
                    case JsonMethodType.DeviceSearch:
                        JsonObjDeviceSearch joDS = new JsonObjDeviceSearch();
                        joDS.method = type;
                        joDS.device = "CMS9999-D5000001";
                        joDS.devicetype = "50";
                        joDS.isexact = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joDS, jsonSetting);
                        break;
                    case JsonMethodType.CameraSearch:
                        JsonObjCameraSearch joCS = new JsonObjCameraSearch();
                        joCS.method = type;
                        joCS.platid = "CMS0001";
                        joCS.device = "办公室设备";
                        joCS.devicetype = "0";
                        joCS.isexact = "0";
                        tempJsonStr = JsonConvert.SerializeObject(joCS, jsonSetting);
                        break;
                    case JsonMethodType.BatAddDeviceFromIP:
                        JsonObjBatAddDeviceFromIP joBADFI = new JsonObjBatAddDeviceFromIP();
                        joBADFI.array = new List<arrayIP>();
                        joBADFI.array.Add(new arrayIP() { ip = "IP地址" });
                        joBADFI.array.Add(new arrayIP() { ip = "IP地址" });
                        joBADFI.devicetype = "2";
                        joBADFI.addmode = "1";
                        joBADFI.loginid = "admin";
                        joBADFI.loginpwd = "123456";
                        joBADFI.rtspflag = "0";
                        joBADFI.allurl = "1";
                        joBADFI.mainurl = "rtsp://admin:12345qwert@X.X.X.X/Streaming/Channels/1";
                        joBADFI.auxurl = "rtsp://admin:12345qwert@X.X.X.X/Streaming/Channels/2";
                        joBADFI.mic = "1";
                        joBADFI.radio = "1";
                        joBADFI.method = type;
                        tempJsonStr = JsonConvert.SerializeObject(joBADFI, jsonSetting);
                        break;
                    case JsonMethodType.BatAddDeviceFromXml:
                        JsonObjBatAddDeviceFromXml joBPDFX = new JsonObjBatAddDeviceFromXml();
                        arrayXml aX = new arrayXml();
                        aX.devicetype = "2";
                        aX.devicename = "设备名1";
                        aX.loginid = "admin";
                        aX.loginpwd = "12345qwert";
                        aX.flag = "0";
                        aX.mainrtsp = "rtsp://admin:12345qwert@10.10.1.200/Streaming/Channels/1";
                        aX.auxrtsp = "rtsp://admin:12345qwert@10.10.1.200/Streaming/Channels/1";
                        aX.devicestate = "可用";
                        aX.note = "备注";
                        aX.mic = "1";
                        aX.radio = "0";
                        joBPDFX.array.Add(aX);
                        joBPDFX.addmode = "2";
                        joBPDFX.method = type;
                        tempJsonStr = JsonConvert.SerializeObject(joBPDFX, jsonSetting);
                        break;
                    case JsonMethodType.BatAddDeviceImport:
                        return "http://[{0}]:[{1}]/xml/upload";
                    case JsonMethodType.BatPingDevice:
                        JsonObjBatPingDevice joBPD = new JsonObjBatPingDevice();
                        joBPD.method = type;
                        joBPD.firstIP = "10.10.1.100";
                        joBPD.lastIP = "10.10.1.200";
                        tempJsonStr = JsonConvert.SerializeObject(joBPD, jsonSetting);
                        break;
                    case JsonMethodType.AlarmSupplier:
                    case JsonMethodType.BASupplier:
                    case JsonMethodType.CVRSupplier:
                    case JsonMethodType.DecoderSupplier:
                    case JsonMethodType.DoorCardSupplier:
                    case JsonMethodType.NetKeySupplier:
                    case JsonMethodType.PSSupplier:
                        JsonObjDeviceId joDI = new JsonObjDeviceId();
                        joDI.method = type;
                        joDI.deviceid = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joDI, jsonSetting);
                        break;
                    case JsonMethodType.BindAlarmCamera:
                        JsonObjBindAlarmCamera joBAC = new JsonObjBindAlarmCamera();
                        joBAC.method = type;
                        joBAC.alarmid = "CMS9999-D5000001";
                        joBAC.areaid = "CMS9999-A000001";
                        joBAC.cameraid = "CMS9999-D2000001";
                        joBAC.alarm_in_channel = "1";
                        joBAC.alarm_out_channel = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joBAC, jsonSetting);
                        break;
                    case JsonMethodType.RebindAlarmCamera:
                        JsonObjRebindAlarmCamera joRBAC = new JsonObjRebindAlarmCamera();
                        joRBAC.method = type;
                        joRBAC.alarmid = "CMS9999-D5000001";
                        joRBAC.cameraid = "CMS9999-D2000001";
                        joRBAC.alarm_in_channel = "1";
                        joRBAC.alarm_out_channel = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joRBAC, jsonSetting);
                        break;
                    case JsonMethodType.GetAlarmCamera:
                        JsonObjGetAlarmCamera joGAC = new JsonObjGetAlarmCamera();
                        joGAC.method = type;
                        joGAC.alarmid = "";
                        joGAC.areaid = "";
                        joGAC.cameraid = "";
                        joGAC.alarm_in_channel = "";
                        joGAC.alarm_out_channel = "";
                        tempJsonStr = JsonConvert.SerializeObject(joGAC, jsonSetting);
                        break;
                    case JsonMethodType.BindBACamera:
                        JsonObjBindBACamera joBBAS = new JsonObjBindBACamera();
                        joBBAS.method = type;
                        joBBAS.deviceid = "CMS9999-D5100001";
                        joBBAS.cameraid = "CMS9999-D2000002";
                        joBBAS.channel = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joBBAS, jsonSetting);
                        break;
                    case JsonMethodType.GetBACamera:
                    case JsonMethodType.DeleteBACamera:
                    case JsonMethodType.GetCVRCamera:
                        JsonObjGetBACamera joGBAC = new JsonObjGetBACamera();
                        joGBAC.method = type;
                        joGBAC.deviceid = "CMS9999-D5100001";
                        joGBAC.cameraid = "CMS9999-D2000002";
                        tempJsonStr = JsonConvert.SerializeObject(joGBAC, jsonSetting);
                        break;
                    case JsonMethodType.OpenBACamera:
                        JsonObjOpenBACamera joOBAC = new JsonObjOpenBACamera();
                        joOBAC.method = type;
                        joOBAC.deviceid = "CMS9999-D5100001";
                        joOBAC.cameraid = "CMS9999-D2000002";
                        joOBAC.channel = "1";
                        joOBAC.state = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joOBAC, jsonSetting);
                        break;
                    case JsonMethodType.BindCVRCamera:
                        JsonObjBindCVRCamera joBCVRC = new JsonObjBindCVRCamera();
                        joBCVRC.method = type;
                        joBCVRC.deviceid = "CMS9999-D5200001";
                        joBCVRC.cameraid = "CMS9999-D2000001";
                        joBCVRC.channel = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joBCVRC, jsonSetting);
                        break;
                    case JsonMethodType.DeleteCVRCamera:
                        JsonObjDeleteCVRCamera joDCVRC = new JsonObjDeleteCVRCamera();
                        joDCVRC.method = type;
                        joDCVRC.deviceid = "CMS9999-D5200001";
                        joDCVRC.cameraid = "CMS9999-D2000001";
                        tempJsonStr = JsonConvert.SerializeObject(joDCVRC, jsonSetting);
                        break;
                    case JsonMethodType.GetCVRFileList:
                        JsonObjGetCVRFileList joGCVRFL = new JsonObjGetCVRFileList();
                        joGCVRFL.method = type;
                        joGCVRFL.deviceid = "CMS9999-D5200001";
                        joGCVRFL.cameraid = "CMS9999-D2000001";
                        joGCVRFL.starttime = "2017-11-11";
                        joGCVRFL.endtime = "2017-12-12";
                        tempJsonStr = JsonConvert.SerializeObject(joGCVRFL, jsonSetting);
                        break;
                    case JsonMethodType.StartDownloadFile:
                        JsonObjStartDownloadFile joSDFile = new JsonObjStartDownloadFile();
                        joSDFile.method = type;
                        joSDFile.deviceid = "CMS9999-D5200001";
                        joSDFile.cameraid = "CMS9999-D2000001";
                        joSDFile.starttime = "2017-11-11 14:45:00";
                        joSDFile.endtime = "2017-12-12 14:45:00";
                        joSDFile.savepath = "/Test/)";
                        joSDFile.savename = "test";
                        tempJsonStr = JsonConvert.SerializeObject(joSDFile, jsonSetting);
                        break;
                    case JsonMethodType.GetDownloadPos:
                        JsonObjGetDownloadPos joGDP = new JsonObjGetDownloadPos();
                        joGDP.method = type;
                        joGDP.deviceid = "CMS9999-D5200001";
                        joGDP.cameraid = "CMS9999-D2000001";
                        joGDP.downhandle = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joGDP, jsonSetting);
                        break;
                    case JsonMethodType.StopDownload:
                        JsonObjStopDownload joSD = new JsonObjStopDownload();
                        joSD.method = type;
                        joSD.deviceid = "CMS9999-D5200001";
                        joSD.cameraid = "CMS9999-D2000001";
                        joSD.downhandle = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joSD, jsonSetting);
                        break;
                    case JsonMethodType.StartDownloadAreaFile:
                        JsonObjStartDownloadAreaFile joSDAFile = new JsonObjStartDownloadAreaFile();
                        joSDAFile.method = type;
                        joSDAFile.areaid = "CMS9999-A000001";
                        joSDAFile.starttime = "2017-09-28 07:40:00";
                        joSDAFile.endtime = "2017-09-28 07:50:00";
                        joSDAFile.savepath = "/Test/";
                        tempJsonStr = JsonConvert.SerializeObject(joSDAFile, jsonSetting);
                        break;
                    case JsonMethodType.PTZControl:
                        JsonObjPTZControl joPTZC = new JsonObjPTZControl();
                        joPTZC.method = type;
                        joPTZC.deviceid = "CMS9999-D5200001";
                        joPTZC.cameraid = "CMS9999-D1000001";
                        joPTZC.cmd = "23";
                        joPTZC.speed = "4";
                        joPTZC.flag = "0";
                        tempJsonStr = JsonConvert.SerializeObject(joPTZC, jsonSetting);
                        break;
                    case JsonMethodType.PTZPreset:
                        JsonObjPTZPreset joPTZP = new JsonObjPTZPreset();
                        joPTZP.method = type;
                        joPTZP.deviceid = "CMS9999-D5200001";
                        joPTZP.cameraid = "CMS9999-D1000001";
                        joPTZP.cmd = "39";
                        joPTZP.index = "33";
                        tempJsonStr = JsonConvert.SerializeObject(joPTZP, jsonSetting);
                        break;
                    case JsonMethodType.GetDoorCardLog:
                        JsonObjGetDoorCardLog joGDCL = new JsonObjGetDoorCardLog();
                        joGDCL.method = type;
                        joGDCL.cardid = "";
                        joGDCL.begtime = "2017-11-11";
                        joGDCL.endtime = "2017-12-12";
                        joGDCL.user = "";
                        joGDCL.area = "";
                        joGDCL.doorcard = "";
                        joGDCL.cardreader = "";
                        tempJsonStr = JsonConvert.SerializeObject(joGDCL, jsonSetting);
                        break;
                    case JsonMethodType.GetSequencerConf:
                        JsonObjGetSequencerConf joGSC = new JsonObjGetSequencerConf();
                        joGSC.method = type;
                        joGSC.area = "CMS9999-A000001";
                        joGSC.sequencer = "CMS9999-D5600001";
                        tempJsonStr = JsonConvert.SerializeObject(joGSC, jsonSetting);
                        break;
                    case JsonMethodType.SequencerConf:
                        JsonObjSequencerConf joSC = new JsonObjSequencerConf();
                        joSC.method = type;
                        joSC.areaid = "CMS9999-A000001";
                        joSC.sequencerid = "CMS9999-D5600001";
                        joSC.bsotype = "1";
                        joSC.openorder = "00000001";
                        joSC.closeorder = "00000001";
                        tempJsonStr = JsonConvert.SerializeObject(joSC, jsonSetting);
                        break;
                    case JsonMethodType.DeleteSequencerConf:
                        JsonObjDeleteSequencerConf joDSC = new JsonObjDeleteSequencerConf();
                        joDSC.method = type;
                        joDSC.areaid = "CMS9999-A000001";
                        joDSC.sequencerid = "CMS9999-D5600001";
                        joDSC.bsotype = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joDSC, jsonSetting);
                        break;
                    case JsonMethodType.SyncRecordSupplier:
                        JsonObjSyncRecordSupplier joSRS = new JsonObjSyncRecordSupplier();
                        joSRS.method = type;
                        joSRS.deviceid = "设备ID";
                        tempJsonStr = JsonConvert.SerializeObject(joSRS, jsonSetting);
                        break;
                    case JsonMethodType.BindSyncRecordCamera:
                        JsonObjBindSyncRecordCamera joBSRC = new JsonObjBindSyncRecordCamera();
                        joBSRC.method = type;
                        joBSRC.deviceid = "CMS9999-D5700001";
                        joBSRC.cameraid = "CMS9999-D2000002";
                        joBSRC.channel = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joBSRC, jsonSetting);
                        break;
                    case JsonMethodType.GetSyncRecordCamera:
                    case JsonMethodType.DeleteSyncRecordCamera:
                        JsonObjDeleteSyncRecordCamera joDSRC = new JsonObjDeleteSyncRecordCamera();
                        joDSRC.method = type;
                        joDSRC.deviceid = "CMS9999-D5700001";
                        joDSRC.cameraid = "CMS9999-D2000002";
                        tempJsonStr = JsonConvert.SerializeObject(joDSRC, jsonSetting);
                        break;
                    case JsonMethodType.AddArea:
                        JsonObjAddArea joAA = new JsonObjAddArea();
                        joAA.method = type;
                        joAA.areaname = "双规室";
                        joAA.areatype = "2";
                        joAA.note = "备注XX";
                        tempJsonStr = JsonConvert.SerializeObject(joAA, jsonSetting);
                        break;
                    case JsonMethodType.DelArea:
                        JsonObjDelArea joDA = new JsonObjDelArea();
                        joDA.method = type;
                        joDA.areaid = "CMS9999-A000004";
                        tempJsonStr = JsonConvert.SerializeObject(joDA, jsonSetting);
                        break;
                    case JsonMethodType.UpdateArea:
                        JsonObjUpdateArea joUA = new JsonObjUpdateArea();
                        joUA.method = type;
                        joUA.areaid = "CMS9999-A000003";
                        joUA.areaname = "第一双规室";
                        joUA.areatype = "2";
                        joUA.note = "备注";
                        tempJsonStr = JsonConvert.SerializeObject(joUA, jsonSetting);
                        break;
                    case JsonMethodType.GetArea:
                        JsonObjGetArea joGA = new JsonObjGetArea();
                        joGA.method = type;
                        joGA.area = "CMS9999-A000003";
                        joGA.isexact = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joGA, jsonSetting);
                        break;
                    case JsonMethodType.BindAreaDevice:
                        JsonObjBindAreaDevice joBAD = new JsonObjBindAreaDevice();
                        joBAD.method = type;
                        joBAD.areaid = "CMS9999-A000002";
                        arrayArea area01 = new arrayArea();
                        area01.deviceid = "CMS9999-D5200001";
                        area01.devicename = "CVR";
                        area01.devicetype = "52";
                        arrayArea area02 = new arrayArea();
                        area02.deviceid = "CMS9999-D5200001";
                        area02.devicename = "摄像头";
                        area02.devicetype = "1";
                        joBAD.array.Add(area01);
                        joBAD.array.Add(area02);
                        tempJsonStr = JsonConvert.SerializeObject(joBAD, jsonSetting);
                        break;
                    case JsonMethodType.GetAreaDevice:
                        JsonObjGetAreaDevice joGAD = new JsonObjGetAreaDevice();
                        joGAD.method = type;
                        joGAD.areaid = "CMS9999-A000001";
                        tempJsonStr = JsonConvert.SerializeObject(joGAD, jsonSetting);
                        break;
                    case JsonMethodType.GetAreaCamera:
                        JsonObjGetAreaCamera joGetAC = new JsonObjGetAreaCamera();
                        joGetAC.method = type;
                        joGetAC.areaid = "";
                        joGetAC.areaname = "";
                        tempJsonStr = JsonConvert.SerializeObject(joGetAC, jsonSetting);
                        break;
                    case JsonMethodType.AddUser:
                        JsonObjAddUser joAU = new JsonObjAddUser();
                        joAU.method = type;
                        joAU.userid = "test";
                        joAU.pwd = "E10ADC3949BA59ABBE56E057F20F883E";
                        joAU.username = "朱欢欢";
                        joAU.telephone = "043112345678";
                        joAU.mobile = "13511112222";
                        joAU.email = "huanhuan.zhu@hedait.cn";
                        arrayUser userAU1 = new arrayUser();
                        userAU1.roleid = "RM000001";
                        joAU.array.Add(userAU1);
                        arrayUser userAU2 = new arrayUser();
                        userAU2.roleid = "RM000002";
                        joAU.array.Add(userAU1);
                        joAU.array.Add(userAU2);
                        tempJsonStr = JsonConvert.SerializeObject(joAU, jsonSetting);
                        break;
                    case JsonMethodType.UpdateUser:
                        JsonObjUpdateUser joUU = new JsonObjUpdateUser();
                        joUU.method = type;
                        joUU.userid = "test";
                        joUU.username = "夏龙龙";
                        joUU.telephone = "043112345678";
                        joUU.mobile = "13511112222";
                        joUU.email = "longlong.xia@hedait.cn";
                        arrayUser userUU1 = new arrayUser();
                        userUU1.roleid = "RM000002";
                        arrayUser userUU2 = new arrayUser();
                        userUU2.roleid = "RM000003";
                        joUU.array.Add(userUU1);
                        joUU.array.Add(userUU2);
                        tempJsonStr = JsonConvert.SerializeObject(joUU, jsonSetting);
                        break;
                    case JsonMethodType.UpdateUserInfo:
                        JsonObjUpdateUserInfo joUUI = new JsonObjUpdateUserInfo();
                        joUUI.method = type;
                        joUUI.userid = "test";
                        joUUI.pwd = "E10ADC3949BA59ABBE56E057F20F883E";
                        joUUI.username = "刘啸啸";
                        joUUI.telephone = "043112345678";
                        joUUI.mobile = "13511112222";
                        joUUI.email = "xiaoxiao.liu@hedait.cn";
                        tempJsonStr = JsonConvert.SerializeObject(joUUI, jsonSetting);
                        break;
                    case JsonMethodType.DeleteUser:
                        JsonObjDeleteUser joDU = new JsonObjDeleteUser();
                        joDU.method = type;
                        joDU.userid = "test";
                        tempJsonStr = JsonConvert.SerializeObject(joDU, jsonSetting);
                        break;
                    case JsonMethodType.UserLogin:
                        JsonObjUserLogin joUL = new JsonObjUserLogin();
                        joUL.method = type;
                        joUL.userid = "test";
                        joUL.pwd = "E10ADC3949BA59ABBE56E057F20F883E";
                        joUL.flag = "0";
                        tempJsonStr = JsonConvert.SerializeObject(joUL, jsonSetting);
                        break;
                    case JsonMethodType.UserSearch:
                    case JsonMethodType.CheckUserID:
                    case JsonMethodType.ResetUserPwd:
                        JsonObjUserSearch joUS = new JsonObjUserSearch();
                        joUS.method = type;
                        joUS.userid = "test";
                        tempJsonStr = JsonConvert.SerializeObject(joUS, jsonSetting);
                        break;
                    case JsonMethodType.GetUser:
                        JsonObjGetUser joGU = new JsonObjGetUser();
                        joGU.method = type;
                        joGU.user = "";
                        joGU.flag = "0";
                        joGU.isexact = "0";
                        tempJsonStr = JsonConvert.SerializeObject(joGU, jsonSetting);
                        break;
                    case JsonMethodType.UpdatePwd:
                        JsonObjUpdatePwd joUP = new JsonObjUpdatePwd();
                        joUP.method = type;
                        joUP.userid = "test";
                        joUP.oldpwd = "F10ADC3949BA59ABBE56E057F20F883E";
                        joUP.pwd = "F10ADC3949BA59ABBE56E057F20F883E";
                        tempJsonStr = JsonConvert.SerializeObject(joUP, jsonSetting);
                        break;
                    case JsonMethodType.BindUserRole:
                        JsonObjBindUserRole joBUR = new JsonObjBindUserRole();
                        joBUR.method = type;
                        joBUR.userid = "test";
                        arrayUser userBUR1 = new arrayUser();
                        userBUR1.roleid = "RM000001";
                        arrayUser userBUR2 = new arrayUser();
                        userBUR2.roleid = "RM000002";
                        joBUR.array.Add(userBUR1);
                        joBUR.array.Add(userBUR2);
                        tempJsonStr = JsonConvert.SerializeObject(joBUR, jsonSetting);
                        break;


                    case JsonMethodType.AddPrivilege:
                        JsonObjAddPrivilege joAP = new JsonObjAddPrivilege();
                        joAP.method = type;
                        joAP.privilegename = "办公室";
                        arrayPrivilege arrAddPrivi1 = new arrayPrivilege();
                        arrAddPrivi1.deviceid = "CMS0001-D2000001";
                        arrAddPrivi1.devicename = "办公室1";
                        arrayPrivilege arrAddPrivi2 = new arrayPrivilege();
                        arrAddPrivi2.deviceid = "CMS0001-D2000002";
                        arrAddPrivi2.devicename = "办公室2";
                        joAP.array.Add(arrAddPrivi1);
                        joAP.array.Add(arrAddPrivi2);
                        tempJsonStr = JsonConvert.SerializeObject(joAP, jsonSetting);
                        break;
                    case JsonMethodType.UpdatePrivilege:
                        JsonObjUpdatePrivilege joUpP = new JsonObjUpdatePrivilege();
                        joUpP.method = type;
                        joUpP.privilegeid = "GM100001";
                        joUpP.privilegename = "办公室";
                        arrayPrivilege arrUpdatePrivi1 = new arrayPrivilege();
                        arrUpdatePrivi1.deviceid = "CMS0001-D2000001";
                        arrUpdatePrivi1.devicename = "办公室3";
                        arrayPrivilege arrUpdatePrivi2 = new arrayPrivilege();
                        arrUpdatePrivi2.deviceid = "CMS0001-D2000002";
                        arrUpdatePrivi2.devicename = "办公室4";
                        joUpP.array.Add(arrUpdatePrivi1);
                        joUpP.array.Add(arrUpdatePrivi2);
                        tempJsonStr = JsonConvert.SerializeObject(joUpP, jsonSetting);
                        break;
                    case JsonMethodType.DeletePrivilege:
                        JsonObjDeletePrivilege joDelP = new JsonObjDeletePrivilege();
                        joDelP.method = type;
                        joDelP.privilegeid = "GM100001";
                        joDelP.privilegename = "办公室";
                        tempJsonStr = JsonConvert.SerializeObject(joDelP, jsonSetting);
                        break;
                    case JsonMethodType.RolePrivilege:
                        JsonObjRolePrivilege joRP = new JsonObjRolePrivilege();
                        joRP.method = type;
                        joRP.roleid = "RM000002";
                        tempJsonStr = JsonConvert.SerializeObject(joRP, jsonSetting);
                        break;
                    case JsonMethodType.PrivilegeDevice:
                        JsonObjPrivilegeDevice joPD = new JsonObjPrivilegeDevice();
                        joPD.method = type;
                        joPD.privilegeid = "GM100001";
                        tempJsonStr = JsonConvert.SerializeObject(joPD, jsonSetting);
                        break;
                    case JsonMethodType.GetPrivilege:
                        JsonObjGetPrivilege joGP = new JsonObjGetPrivilege();
                        joGP.method = type;
                        joGP.flag = "0";
                        joGP.privilege = "办公室";
                        joGP.isexact = "0";
                        tempJsonStr = JsonConvert.SerializeObject(joGP, jsonSetting);
                        break;
                    case JsonMethodType.AddRole:
                        JsonObjAddRole joAR = new JsonObjAddRole();
                        joAR.method = type;
                        joAR.rolename = "办公室管理";
                        arrayRole arrAddRole = new arrayRole();
                        arrAddRole.privilegeid = "GM100001";
                        arrAddRole.privilegename = "办公室";
                        joAR.array.Add(arrAddRole);
                        tempJsonStr = JsonConvert.SerializeObject(joAR, jsonSetting);
                        break;
                    case JsonMethodType.UpdateRole:
                        JsonObjUpdateRole joUR = new JsonObjUpdateRole();
                        joUR.method = type;
                        joUR.rolename = "办公室管理2";
                        joUR.roleid = "RM100001";
                        arrayRole arrRole = new arrayRole();
                        arrRole.privilegeid = "GM100001";
                        arrRole.privilegename = "办公室";
                        joUR.array.Add(arrRole);
                        tempJsonStr = JsonConvert.SerializeObject(joUR, jsonSetting);
                        break;
                    case JsonMethodType.DeleteRole:
                        JsonObjDeleteRole joDR = new JsonObjDeleteRole();
                        joDR.method = type;
                        joDR.rolename = "办公室管理";
                        joDR.roleid = "RM100001";
                        tempJsonStr = JsonConvert.SerializeObject(joDR, jsonSetting);
                        break;
                    case JsonMethodType.UserRole:
                        JsonObjUserRole joUserR = new JsonObjUserRole();
                        joUserR.method = type;
                        joUserR.userid = "user_1024";
                        tempJsonStr = JsonConvert.SerializeObject(joUserR, jsonSetting);
                        break;
                    case JsonMethodType.GetRole:
                        JsonObjGetRole joGetR = new JsonObjGetRole();
                        joGetR.method = type;
                        joGetR.role = "";
                        joGetR.isexact = "0";
                        tempJsonStr = JsonConvert.SerializeObject(joGetR, jsonSetting);
                        break;
                    case JsonMethodType.AddPlat:
                        JsonObjAddPlat joAddP = new JsonObjAddPlat();
                        joAddP.method = type;
                        joAddP.port = "555";
                        joAddP.remoteip = "10.10.1.207";
                        tempJsonStr = JsonConvert.SerializeObject(joAddP, jsonSetting);
                        break;
                    case JsonMethodType.UpdatePlat:
                        JsonObjUpdatePlat joUPlat = new JsonObjUpdatePlat();
                        joUPlat.method = type;
                        joUPlat.ip = "10.10.1.207";
                        joUPlat.port = "556";
                        joUPlat.platid = "CMS0002";
                        tempJsonStr = JsonConvert.SerializeObject(joUPlat, jsonSetting);
                        break;
                    case JsonMethodType.DeletePlat:
                        JsonObjDeletePlat joDPlat = new JsonObjDeletePlat();
                        joDPlat.method = type;
                        joDPlat.platid = "CMS0002";
                        tempJsonStr = JsonConvert.SerializeObject(joDPlat, jsonSetting);
                        break;
                    case JsonMethodType.PlatListExact:
                        JsonObjPlatListExact joPL = new JsonObjPlatListExact();
                        joPL.method = type;
                        joPL.plat = "CMS0002";
                        joPL.isexact = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joPL, jsonSetting);
                        break;
                    case JsonMethodType.AddGroup:
                        JsonObjAddGroup joAG = new JsonObjAddGroup();
                        joAG.method = type;
                        joAG.groupname = "办公室";
                        joAG.parentgroupid = "";
                        tempJsonStr = JsonConvert.SerializeObject(joAG, jsonSetting);
                        break;
                    case JsonMethodType.UpdateGroup:
                        JsonObjUpdateGroup joUG = new JsonObjUpdateGroup();
                        joUG.method = type;
                        joUG.groupname = "办公室";
                        joUG.parentgroupid = "GP000001";
                        joUG.groupid = "GP000003";
                        tempJsonStr = JsonConvert.SerializeObject(joUG, jsonSetting);
                        break;
                    case JsonMethodType.DeleteGroup:
                        JsonObjDeleteGroup joDG = new JsonObjDeleteGroup();
                        joDG.method = type;
                        joDG.groupid = "GP000003";
                        tempJsonStr = JsonConvert.SerializeObject(joDG, jsonSetting);
                        break;
                    case JsonMethodType.PlatGroup:
                        JsonObjPlatGroup joPG = new JsonObjPlatGroup();
                        joPG.method = type;
                        joPG.platid = "CMS0001";
                        tempJsonStr = JsonConvert.SerializeObject(joPG, jsonSetting);
                        break;
                    case JsonMethodType.GroupSearch:
                        JsonObjGroupSearch joGS = new JsonObjGroupSearch();
                        joGS.method = type;
                        joGS.group = "CMS0002-GP000003";
                        joGS.isexact = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joGS, jsonSetting);
                        break;
                    case JsonMethodType.KeepMove:
                        JsonObjKeepMove joKM = new JsonObjKeepMove();
                        joKM.method = type;
                        joKM.deviceid = "MS0002-D2000001";
                        joKM.x = "0.5";
                        joKM.y = "0";
                        joKM.z = "0";
                        tempJsonStr = JsonConvert.SerializeObject(joKM, jsonSetting);
                        break;
                    case JsonMethodType.StopMove:
                        JsonObjStopMove joSM = new JsonObjStopMove();
                        joSM.method = type;
                        joSM.deviceid = "CMS0002-D2000001";
                        joSM.xyz = "x";
                        tempJsonStr = JsonConvert.SerializeObject(joSM, jsonSetting);
                        break;
                    case JsonMethodType.RltMove:
                        JsonObjRltMove joRltM = new JsonObjRltMove();
                        joRltM.method = type;
                        joRltM.deviceid = "CMS0002-D2000001";
                        joRltM.x = "0.5";
                        joRltM.y = "0";
                        joRltM.z = "0";
                        tempJsonStr = JsonConvert.SerializeObject(joRltM, jsonSetting);
                        break;
                    case JsonMethodType.GetPresets:
                        JsonObjGetPresets joGetP = new JsonObjGetPresets();
                        joGetP.method = type;
                        joGetP.deviceid = "CMS0002-D2000001";
                        tempJsonStr = JsonConvert.SerializeObject(joGetP, jsonSetting);
                        break;
                    case JsonMethodType.SetPresets:
                    case JsonMethodType.RemovePresets:
                    case JsonMethodType.GotoPresets:
                        JsonObjSetPresets joSP = new JsonObjSetPresets();
                        joSP.method = type;
                        joSP.deviceid = "CMS0002-D2000001";
                        joSP.name = "预置点设置1";
                        joSP.Token = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joSP, jsonSetting);
                        break;
                    case JsonMethodType.RtspRelay:
                        JsonObjRtspRelay joRtspR = new JsonObjRtspRelay();
                        joRtspR.method = type;
                        joRtspR.deviceid = "CMS0001-D2000001";
                        joRtspR.flag = "0";
                        tempJsonStr = JsonConvert.SerializeObject(joRtspR, jsonSetting);
                        break;
                    case JsonMethodType.TSFileRelay:
                        JsonObjTSFileRelay joTSFR = new JsonObjTSFileRelay();
                        joTSFR.method = type;
                        joTSFR.deviceid = "CMS0002/CMS0002-D2000001";
                        joTSFR.begtime = "2017-02-17 14:10:00";
                        joTSFR.endtime = "2017-02-17 22:10:00";
                        tempJsonStr = JsonConvert.SerializeObject(joTSFR, jsonSetting);
                        break;
                    case JsonMethodType.TSFileSeekPlay:
                        JsonObjTSFileSeekPlay joTSFSP = new JsonObjTSFileSeekPlay();
                        joTSFSP.method = type;
                        joTSFSP.deviceid = "CMS0002/CMS0002-D2000001";
                        joTSFSP.videoname = "CMS0002/CMS0002-D2000001-1495705633";
                        joTSFSP.videopos = "60";
                        tempJsonStr = JsonConvert.SerializeObject(joTSFSP, jsonSetting);
                        break;
                    case JsonMethodType.DownloadFileList:
                        JsonObjDownloadFileList joDFL = new JsonObjDownloadFileList();
                        joDFL.method = type;
                        joDFL.deviceid = "CMS0001-D1000001";
                        joDFL.begtime = "2017-02-17 14:10:00";
                        joDFL.endtime = "2017-02-17 18:10:00";
                        tempJsonStr = JsonConvert.SerializeObject(joDFL, jsonSetting);
                        break;
                    case JsonMethodType.GetRelayRtsp:
                        JsonObjGetRelayRtsp joGRR = new JsonObjGetRelayRtsp();
                        joGRR.method = type;
                        joGRR.deviceid = "CMS0001-D1000001";
                        joGRR.audio = "1";
                        joGRR.flag = "1";
                        tempJsonStr = JsonConvert.SerializeObject(joGRR, jsonSetting);
                        break;
                    case JsonMethodType.AddPlan:
                    case JsonMethodType.UpdatePlan:
                        JsonObjAddPlan joAPlan = new JsonObjAddPlan();
                        joAPlan.method = type;
                        joAPlan.deviceid = "CMS0001-D2000007";
                        joAPlan.begdate = "2017-01-18";
                        joAPlan.enddate = "2017-01-18";
                        joAPlan.begloop = "";
                        joAPlan.endloop = "";
                        joAPlan.begtime = "08:00:00";
                        joAPlan.endtime = "18:00:00";
                        joAPlan.loopflag = "0";
                        joAPlan.tasktype = "0";
                        joAPlan.userid = "test";
                        joAPlan.planname = "巡航";
                        tempJsonStr = JsonConvert.SerializeObject(joAPlan, jsonSetting);
                        break;
                    case JsonMethodType.PlanSearch:
                        JsonObjPlanSearch joPlanS = new JsonObjPlanSearch();
                        joPlanS.method = type;
                        joPlanS.plan = "CMS0001-D2000007";
                        tempJsonStr = JsonConvert.SerializeObject(joPlanS, jsonSetting);
                        break;
                    case JsonMethodType.ExactPlanSearch:
                    case JsonMethodType.DeletePlan:
                        JsonObjExactPlanSearch joEPS = new JsonObjExactPlanSearch();
                        joEPS.method = type;
                        joEPS.deviceid = "CMS0001-D2000007";
                        joEPS.planname = "录像";
                        tempJsonStr = JsonConvert.SerializeObject(joEPS, jsonSetting);
                        break;
                    case JsonMethodType.ClosePlan:
                        JsonObjClosePlan joCP = new JsonObjClosePlan();
                        joCP.method = type;
                        joCP.deviceid = "CMS0001-D2000007";
                        joCP.planname = "巡航";
                        joCP.state = "0";
                        tempJsonStr = JsonConvert.SerializeObject(joCP, jsonSetting);
                        break;
                    case JsonMethodType.BatAddPlan:
                        JsonObjBatAddPlan joBAP = new JsonObjBatAddPlan();
                        joBAP.method = type;
                        joBAP.method = type;
                        joBAP.deviceid = "CMS0001-D2000007";
                        joBAP.begdate = "2017-01-18";
                        joBAP.enddate = "2017-01-18";
                        joBAP.begloop = ""; 
                        joBAP.endloop = "";
                        joBAP.begtime = "08:00:00";
                        joBAP.endtime = "18:00:00";
                        joBAP.loopflag = "0";
                        joBAP.tasktype = "0";
                        joBAP.userid = "test";
                        arrayPlan arrPlan = new arrayPlan();
                        arrPlan.planname = "CMS0001-D2000002";
                        arrPlan.deviceid = "录像";
                        tempJsonStr = JsonConvert.SerializeObject(joBAP, jsonSetting);
                        break;
                    case JsonMethodType.MP4Preview:
                        JsonObjMP4Preview joMP4P = new JsonObjMP4Preview();
                        joMP4P.method = type;
                        joMP4P.loginid = "admin";
                        joMP4P.loginpwd = "123456";
                        joMP4P.url = "rtsp://admin:12345qwert@10.10.1.200/Streaming/Channels/1";
                        tempJsonStr = JsonConvert.SerializeObject(joMP4P, jsonSetting);
                        break;
                    case JsonMethodType.DevicePreview:
                        JsonObjDevicePreview joDPre = new JsonObjDevicePreview();
                        joDPre.method = type;
                        joDPre.deviceid = "CMS0002-D2000001";
                        joDPre.flag = "0";
                        tempJsonStr = JsonConvert.SerializeObject(joDPre, jsonSetting);
                        break;
                    case JsonMethodType.RMSConf:
                        JsonObjRMSConf joRMSC = new JsonObjRMSConf();
                        joRMSC.method = type;
                        joRMSC.ip = "127.0.0.1";
                        joRMSC.port = "1199";
                        joRMSC.retain_time = "72";
                        joRMSC.src_num = "9999";
                        joRMSC.reconn_time = "30";
                        joRMSC.save_type = "0";
                        joRMSC.length = "30";
                        joRMSC.section = "3";
                        joRMSC.save_pos = "0";
                        joRMSC.local_path = "D:\TestFolder\";
                        joRMSC.leave_space = "80";
                        joRMSC.over_opt = "0";
                        tempJsonStr = JsonConvert.SerializeObject(joRMSC, jsonSetting);
                        break;
                    case JsonMethodType.DBConf:
                    case JsonMethodType.CheckDB:
                        JsonObjDBConf joDBConf = new JsonObjDBConf();
                        joDBConf.method = type;
                        joDBConf.ip = "127.0.0.1";
                        joDBConf.port = "3306";
                        joDBConf.dbname = "hedait_see_plus";
                        joDBConf.dbuser = "hedait";
                        joDBConf.dbpwd = "heda1t12138";
                        tempJsonStr = JsonConvert.SerializeObject(joDBConf, jsonSetting);
                        break;
                    case JsonMethodType.ReStart:
                        JsonObjReStart joReStart = new JsonObjReStart();
                        joReStart.method = type;
                        joReStart.server = "CMS";
                        tempJsonStr = JsonConvert.SerializeObject(joReStart, jsonSetting);
                        break;
                    case JsonMethodType.AllDataSync:
                        tempJsonStr = CreateJsonObjAllDataSync(type,jsonSetting);
                        break;
                    case JsonMethodType.DataSync:
                        tempJsonStr = CreateJsonObjDataSync(type, jsonSetting);
                        break;
                    default:
                        break;
                }
                //tempJsonStr = JsonConvert.SerializeObject(tempObj, jsonSetting);
                temp = ConvertJsonString(tempJsonStr);
                return temp;
            }
            catch (System.Exception ex)
            {
                return string.Format("An error occurred ", ex.Message);
            }
        }

        private string CreateJsonObjAllDataSync(string type,JsonSerializerSettings jsonSetting)
        {
            try
            {
                JsonObjAllDataSync joADS = new JsonObjAllDataSync();
                joADS.method = type;
                joADS.count = "同步表数量";
                joADS.table_1 = "同步表1的表名";
                joADS.table_2 = "同步表2的表名";
                joADS.table_3 = "同步表3的表名";
                arrayDataSync1 arrDS1 = new arrayDataSync1();
                arrayDataSync2 arrDS2 = new arrayDataSync2();
                arrayDataSync3 arrDS3 = new arrayDataSync3();
                joADS.array_1.Add(arrDS1);
                joADS.array_2.Add(arrDS2);
                joADS.array_3.Add(arrDS3);
                return JsonConvert.SerializeObject(joADS, jsonSetting);
            }
            catch (System.Exception ex)
            {
                return "Create JAddDataSync's Json Object Failed.";
            }
        }

        private string CreateJsonObjDataSync(string type, JsonSerializerSettings jsonSetting)
        {
            try
            {
                JsonObjDataSync joADS = new JsonObjDataSync();
                joADS.method = type;
                joADS.count = "同步表数量";
                joADS.table_1 = "同步表1的表名";
                joADS.array_1 = new List<string>() { "..."};
                return JsonConvert.SerializeObject(joADS, jsonSetting);
            }
            catch (System.Exception ex)
            {
                return "Create JAddDataSync's Json Object Failed.";
            }
        }

        /// <summary>
        /// 格式化json字符串，添加 "\r\n"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ConvertJsonString(string str)
        {
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }
    }

}
