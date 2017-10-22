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
                JsonMethodType a = (JsonMethodType)Enum.Parse(typeof(JsonMethodType), type);
                JsonSerializerSettings jsonSetting = new JsonSerializerSettings();
                //解析时忽略Null Value的属性
                jsonSetting.NullValueHandling = NullValueHandling.Ignore;
                //可以解析继承类
                jsonSetting.TypeNameHandling = TypeNameHandling.All;
                jsonSetting.ConstructorHandling = ConstructorHandling.Default;
                //Json String解压缩
                string tempJsonStr = string.Empty;
                switch (a)
                {
                    case JsonMethodType.Version:
                        JsonObjVersion joV = new JsonObjVersion();
                        joV.method = type;
                        tempJsonStr = JsonConvert.SerializeObject(joV, jsonSetting);
                        break;
                    case JsonMethodType.RunStatus:
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

                        break;
                    case JsonMethodType.DeleteCamera:

                        break;
                    case JsonMethodType.UpdateDevice:

                        break;
                    case JsonMethodType.UpdateCamera:

                        break;
                    case JsonMethodType.DeviceSearch:

                        break;
                    case JsonMethodType.CameraSearch:

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
                        jsonSetting.TypeNameHandling = TypeNameHandling.Auto; 
                        tempJsonStr = JsonConvert.SerializeObject(joBADFI, jsonSetting);
                        break;
                    case JsonMethodType.BatAddDeviceFromXml:
                        JsonObjBatAddDeviceFromXml joBPDFX = new JsonObjBatAddDeviceFromXml();
                        joBPDFX.method = type;
                        joBPDFX.addmode = "批量添加类型，0-onvif扫描添加、1-手动ip段添加、2-xml文件导入添加";
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
                        jsonSetting.TypeNameHandling = TypeNameHandling.Auto;
                        tempJsonStr = JsonConvert.SerializeObject(joBPDFX, jsonSetting);
                        break;
                    case JsonMethodType.BatPingDevice:
                        return "http://[ip]:[port]/xml/upload";
                    case JsonMethodType.SaveDevFile:
                        JsonObjSaveDevFile joSDF = new JsonObjSaveDevFile();
                        joSDF.method = type;
                        break;
                    case JsonMethodType.AlarmSupplier:

                        break;
                    case JsonMethodType.BindAlarmCamera:

                        break;
                    case JsonMethodType.RebindAlarmCamera:

                        break;
                    case JsonMethodType.GetAlarmCamera:

                        break;
                    case JsonMethodType.BASupplier:

                        break;
                    case JsonMethodType.BindBACamera:

                        break;
                    case JsonMethodType.GetBACamera:

                        break;
                    case JsonMethodType.DeleteBACamera:

                        break;
                    case JsonMethodType.OpenBACamera:

                        break;
                    case JsonMethodType.CVRSupplier:

                        break;
                    case JsonMethodType.BindCVRCamera:

                        break;
                    case JsonMethodType.GetCVRCamera:

                        break;
                    case JsonMethodType.StartDownloadFile:

                        break;
                    case JsonMethodType.PTZControl:

                        break;
                    case JsonMethodType.PTZPreset:

                        break;
                    case JsonMethodType.DecoderSupplier:

                        break;
                    case JsonMethodType.DoorCardSupplier:

                        break;
                    case JsonMethodType.GetDoorCardLog:

                        break;
                    case JsonMethodType.NetKeySupplier:

                        break;
                    case JsonMethodType.PSSupplier:

                        break;
                    case JsonMethodType.GetSequencerConf:

                        break;
                    case JsonMethodType.SequencerConf:

                        break;
                    case JsonMethodType.DeleteSequencerConf:

                        break;
                    case JsonMethodType.SyncRecordSupplier:

                        break;
                    case JsonMethodType.BindSyncRecordCamera:

                        break;
                    case JsonMethodType.GetSyncRecordCamera:

                        break;
                    case JsonMethodType.DeleteSyncRecordCamera:

                        break;
                    case JsonMethodType.AddArea:

                        break;
                    case JsonMethodType.DelArea:

                        break;
                    case JsonMethodType.UpdateArea:

                        break;
                    case JsonMethodType.GetArea:

                        break;
                    case JsonMethodType.BindAreaDevice:

                        break;
                    case JsonMethodType.GetAreaDevice:

                        break;
                    case JsonMethodType.GetAreaCamera:

                        break;
                    case JsonMethodType.AddUser:

                        break;
                    case JsonMethodType.UpdateUser:

                        break;
                    case JsonMethodType.UpdateUserInfo:

                        break;
                    case JsonMethodType.DeleteUser:

                        break;
                    case JsonMethodType.UserLogin:

                        break;
                    case JsonMethodType.UserSearch:

                        break;
                    case JsonMethodType.CheckUserID:

                        break;
                    case JsonMethodType.ResetUserPwd:

                        break;
                    case JsonMethodType.GetUser:

                        break;
                    case JsonMethodType.UpdatePwd:

                        break;
                    case JsonMethodType.BindUserRole:

                        break;
                    default:
                        break;
                }

                temp = ConvertJsonString(tempJsonStr);
                return temp;
            }
            catch (System.Exception ex)
            {
                return string.Format("An error occurred ", ex.Message);
            }
        }

        /// <summary>
        /// 格式化json字符串
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
