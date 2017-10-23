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
                    case JsonMethodType.SaveDevFile:
                        JsonObjVersion joRS = new JsonObjVersion();
                        joRS.method = type;
                        tempJsonStr = JsonConvert.SerializeObject(joRS, jsonSetting);
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
