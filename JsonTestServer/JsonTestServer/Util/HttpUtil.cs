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
            try
            {
                //Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                //byte[] postDataByte = encode.GetBytes(postDataStr);
                byte[] postDataByte = System.Text.Encoding.Default.GetBytes(postDataStr);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
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
            try
            {
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                byte[] postDataByte = encode.GetBytes(postDataStr);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
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
                MessageBox.Show(ex.Message);
                return "";
            }

        }

        public string HttpGet(string Url, string postDataStr)
        {
            string retString = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
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
                MessageBox.Show(ex.Message);
                return "";
            }

        }

        /// <summary>
        /// 通过对应的Json对象返回对应的Json String
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected internal string JsonObjectCreator(string type)
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
                        jrULS.begtime = "开始时间，格式：YYYY-MM-DD";
                        jrULS.endtime = "结束时间，格式：YYYY-MM-DD";
                        jrULS.user = "用户名或用户ID";
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
                        joAC.devicename = "设备名";
                        joAC.bindid = "绑定设备ID";
                        joAC.groupid = "上级组ID";
                        joAC.loginid = "设备登录ID";
                        joAC.loginpwd = "设备登录密码";
                        joAC.ip = "设备IP";
                        joAC.port = "设备端口";
                        joAC.mainrtsp = "主流地址";
                        joAC.auxrtsp = "辅流地址";
                        joAC.flag = "使用主辅流标记";
                        joAC.devicestate = "设备状态";
                        joAC.note = "备注";
                        joAC.mic = "拾音器";
                        joAC.radio = "扬声器";
                        tempJsonStr = JsonConvert.SerializeObject(joAC, jsonSetting);
                        break;
                    case JsonMethodType.DelDevice:
                        JsonObjDelDevice joDD = new JsonObjDelDevice();
                        joDD.method = type;
                        joDD.deviceid = "设备唯一标识";
                        tempJsonStr = JsonConvert.SerializeObject(joDD, jsonSetting);
                        break;
                    case JsonMethodType.DeleteCamera:
                        JsonObjDeleteCamera joDC = new JsonObjDeleteCamera();
                        joDC.method = type;
                        joDC.deviceid = "设备ID";
                        joDC.devicename = "设备名";
                        joDC.delrecord = "删除录像文件";
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
                        joBADFI.mic = "拾音器";
                        joBADFI.radio = "扬声器";
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
                        joBPD.firstIP = "";
                        joBPD.lastIP = "";
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
                        joGAC.alarmid = "报警主机ID";
                        joGAC.areaid = "房间ID";
                        joGAC.cameraid = "摄像头ID";
                        joGAC.alarm_in_channel = "报警主机输入口";
                        joGAC.alarm_out_channel = "报警主机输出口";
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
                        joOBAC.state = "状态";
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
                        joGCVRFL.deviceid = "CVR ID";
                        joGCVRFL.cameraid = "摄像头ID";
                        joGCVRFL.starttime = "开始时间，格式(yyyy-MM-dd HH:MM:SS)";
                        joGCVRFL.endtime = "结束时间，格式(yyyy-MM-dd HH:MM:SS)";
                        tempJsonStr = JsonConvert.SerializeObject(joGCVRFL, jsonSetting);
                        break;
                    case JsonMethodType.StartDownloadFile:
                        JsonObjStartDownloadFile joSDFile = new JsonObjStartDownloadFile();
                        joSDFile.method = type;
                        joSDFile.deviceid = "CVR ID";
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
                        joGDP.deviceid = "CVR ID";
                        joGDP.cameraid = "摄像头ID";
                        joGDP.downhandle = "下载标记";
                        tempJsonStr = JsonConvert.SerializeObject(joGDP, jsonSetting);
                        break;
                    case JsonMethodType.StopDownload:
                        JsonObjStopDownload joSD = new JsonObjStopDownload();
                        joSD.method = type;
                        joSD.deviceid = "存储设备 ID";
                        joSD.cameraid = "摄像头ID";
                        joSD.downhandle = "下载标记";
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
                        joPTZC.speed = "速度";
                        joPTZC.flag = "开始关闭标记";
                        tempJsonStr = JsonConvert.SerializeObject(joPTZC, jsonSetting);
                        break;
                    case JsonMethodType.PTZPreset:
                        JsonObjPTZPreset joPTZP = new JsonObjPTZPreset();
                        joPTZP.method = type;
                        joPTZP.deviceid = "存储设备ID";
                        joPTZP.cameraid = "摄像头ID";
                        joPTZP.cmd = "命令";
                        joPTZP.index= "预置点序号";
                        tempJsonStr = JsonConvert.SerializeObject(joPTZP, jsonSetting);
                        break;
                    case JsonMethodType.GetDoorCardLog:
                        JsonObjGetDoorCardLog joGDCL = new JsonObjGetDoorCardLog();
                        joGDCL.method = type;
                        joGDCL.cardid = "门卡ID";
                        joGDCL.begtime = "开始时间";
                        joGDCL.endtime = "结束时间";
                        joGDCL.user = "刷卡用户名或ID";
                        joGDCL.area = "房间名或ID";
                        joGDCL.doorcard = "门禁主机名或ID";
                        joGDCL.cardreader = "门禁读卡器名或ID";
                        tempJsonStr = JsonConvert.SerializeObject(joGDCL, jsonSetting);
                        break;                       
                    case JsonMethodType.GetSequencerConf:
                        JsonObjGetSequencerConf joGSC = new JsonObjGetSequencerConf();
                        joGSC.method = type;
                        joGSC.area = "房间ID";
                        joGSC.sequencer = "电源时序器";
                        tempJsonStr = JsonConvert.SerializeObject(joGSC, jsonSetting);
                        break;
                    case JsonMethodType.SequencerConf:
                        JsonObjSequencerConf joSC = new JsonObjSequencerConf();
                        joSC.method = type;
                        joSC.areaid= "房间ID";
                        joSC.sequencerid = "电源时序器ID";
                        joSC.bsotype = "业务模块类型";
                        joSC.openorder = "启动顺序";
                        joSC.closeorder = "关闭顺序";
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
                        joAU.userid = "用户ID";
                        joAU.pwd = "用户密码";
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
                        joUU.userid = "用户ID";
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
                        joUUI.pwd = "用户密码";
                        joUUI.username = "真实姓名";
                        joUUI.telephone = "办公室电话";
                        joUUI.mobile = "移动电话";
                        joUUI.email = "电子邮件";
                        tempJsonStr = JsonConvert.SerializeObject(joUUI, jsonSetting);
                        break;
                    case JsonMethodType.DeleteUser:
                        JsonObjDeleteUser joDU = new JsonObjDeleteUser();
                        joDU.method = type;
                        joDU.userid = "用户ID";
                        tempJsonStr = JsonConvert.SerializeObject(joDU, jsonSetting);
                        break;
                    case JsonMethodType.UserLogin:
                        JsonObjUserLogin joUL = new JsonObjUserLogin();
                        joUL.method = type;
                        joUL.userid = "用户ID";
                        joUL.pwd = "用户密码";
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
                        joBUR.userid = "用户ID";
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
