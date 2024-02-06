using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;
using System.Text;
using HalconDotNet;
using System;
using System.IO;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Nodes;
using Microsoft.VisualBasic;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Numerics;
using Newtonsoft.Json.Linq;


public class Program
{

    public static String decodedString = "";
    public static String barCodeString = "";
    public static async Task Main(string[] args)
    {
        var localIP = GetLocalIpAddress2();
        var cUrl = "http://" + localIP + ":8080";
        string[] urls;
        if (localIP == null || localIP.Length < 1)
        {
            urls = ["http://localhost:8080"];
        }
        else
        {
            urls = ["http://localhost:8080", cUrl];
        }
        Console.WriteLine(localIP + " : IP");
        var host = new WebHostBuilder()
            .UseKestrel()
            .UseUrls(urls)
            .ConfigureServices(services => services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>().AddCors(options => options.AddPolicy("CorsPolicy",
                builder => builder
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyHeader())))
            .Configure(app =>
            {
                app.UseCors("CorsPolicy");
                app.Run(async context =>
                {
                    //  Console.WriteLine("Called");
                    if (context.Request.Path == "/api/file")
                    {
                        // Console.WriteLine("Called2");
                        // var file = context.Request.Form.Files[0];
                        // var fileSize = file.Length;
                        // Console.WriteLine("result : " + fileSize.ToString());
                        // await context.Response.WriteAsync(fileSize.ToString() + "some");

                        if (context.Request.Method == "POST")
                        {

                            var formCollection = await context.Request.ReadFormAsync();
                            var file = context.Request.Form.Files[0];
                            var data = formCollection["data"];
                            // Convert the JSON string to a JObject
                            JObject jsonObject = JObject.Parse(data);

                            // Access individual items
                            Boolean rplane = (Boolean)jsonObject["rplane"];
                            float multi_factor = (float)jsonObject["multi_factor"];
                            Boolean contrast = (Boolean)jsonObject["contrast"];

                            // Display the extracted items
                            Console.WriteLine($"rplane: {rplane}");
                            Console.WriteLine($"multi_factor: {multi_factor}");
                            Console.WriteLine($"contrast: {contrast}");
                            //  Console.WriteLine(data.ToString());
                            var fileName = Path.GetRandomFileName();
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName + ".png");

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            var fileSize = file.Length;
                            String jsx = Action(filePath, rplane, contrast);
                            //var jso = Action(filePath);
                            // String jsx = (string)jso["grades"];
                            // String decodeds = (string)jso["decoded"];
                            // Console.WriteLine("Decoded: " + decoded);
                            Console.WriteLine("JSON  DATA : " + jsx);

                            var fileInfo = new
                            {
                                FileName = fileName,
                                FilePath = filePath,
                                jsonData = jsx,
                                decoded = decodedString
                            };


                            var json = JsonConvert.SerializeObject(fileInfo);
                            //var json = JsonConvert.SerializeObject(jso);
                            context.Response.ContentType = "application/json";

                            //File.Delete(filePath);

                            await context.Response.WriteAsync(json, Encoding.UTF8);
                        }
                    }
                    else if (context.Request.Path == "/api/fileCopy")
                    {


                        if (context.Request.Method == "POST")
                        {
                            var formCollection = await context.Request.ReadFormAsync();
                            var data = formCollection["data"];
                            // Convert the JSON string to a JObject
                            JObject jsonObject = JObject.Parse(data);

                            // Access individual items
                            Boolean rplane = (Boolean)jsonObject["rplane"];
                            float multi_factor = (float)jsonObject["multi_factor"];
                            Boolean contrast = (Boolean)jsonObject["contrast"];

                            // Display the extracted items
                            Console.WriteLine($"rplane: {rplane}");
                            Console.WriteLine($"multi_factor: {multi_factor}");
                            Console.WriteLine($"contrast: {contrast}");
                            var file = context.Request.Form.Files[0];
                            var fileName = Path.GetRandomFileName();
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName + ".png");

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            var file2 = context.Request.Form.Files[0];
                            var fileName2 = Path.GetRandomFileName();
                            var filePath2 = Path.Combine(Directory.GetCurrentDirectory(), fileName2 + ".png");

                            using (var stream = new FileStream(filePath2, FileMode.Create))
                            {
                                await file2.CopyToAsync(stream);
                            }
                            var file3 = context.Request.Form.Files[0];
                            var fileName3 = Path.GetRandomFileName();
                            var filePath3 = Path.Combine(Directory.GetCurrentDirectory(), fileName3 + ".png");

                            using (var stream = new FileStream(filePath3, FileMode.Create))
                            {
                                await file3.CopyToAsync(stream);
                            }
                            var fileSize = file.Length;
                            String jsx1 = Action(filePath, rplane, contrast);
                            String jsx2 = Action(filePath, rplane, contrast);
                            String jsx3 = Action(filePath, rplane, contrast);
                            //var jso = Action(filePath);
                            // String jsx = (string)jso["grades"];
                            // String decodeds = (string)jso["decoded"];
                            // Console.WriteLine("Decoded: " + decoded);
                            // Console.WriteLine("JSON  DATA : " + jsx);

                            var fileInfo = new
                            {
                                FileName = fileName,
                                FilePath = filePath,
                                jsonData = jsx1,
                                jsonData2 = jsx2,
                                jsonData3 = jsx3,
                                decoded = decodedString
                            };

                            //  Console.WriteLine(decodedString + ": DEDE");
                            // Console.WriteLine(fileInfo.ToString());

                            var json = JsonConvert.SerializeObject(fileInfo);
                            //var json = JsonConvert.SerializeObject(jso);
                            context.Response.ContentType = "application/json";

                            File.Delete(filePath);
                            File.Delete(filePath2);
                            File.Delete(filePath3);

                            await context.Response.WriteAsync(json, Encoding.UTF8);
                        }
                    }
                    else
                    if (context.Request.Path == "/api/fileNorm")
                    {


                        if (context.Request.Method == "POST")
                        {
                            var formCollection = await context.Request.ReadFormAsync();
                            var data = formCollection["data"];
                            // Convert the JSON string to a JObject
                            JObject jsonObject = JObject.Parse(data);

                            // Access individual items
                            Boolean rplane = (Boolean)jsonObject["rplane"];
                            float multi_factor = (float)jsonObject["multi_factor"];
                            Boolean contrast = (Boolean)jsonObject["contrast"];

                            // Display the extracted items
                            Console.WriteLine($"rplane: {rplane}");
                            Console.WriteLine($"multi_factor: {multi_factor}");
                            Console.WriteLine($"contrast: {contrast}");

                            var file = context.Request.Form.Files[0];
                            var fileName = Path.GetRandomFileName();
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName + ".png");

                            var file2 = context.Request.Form.Files[1];
                            var fileName2 = Path.GetRandomFileName();
                            var filePath2 = Path.Combine(Directory.GetCurrentDirectory(), fileName2 + ".png");

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            var fileSize = file.Length;

                            using (var stream2 = new FileStream(filePath2, FileMode.Create))
                            {
                                await file2.CopyToAsync(stream2);
                            }
                            var fileSize2 = file2.Length;


                            String jsx = ActionNorm(filePath, filePath2, rplane, contrast, multi_factor);
                            //var jso = Action(filePath);
                            // String jsx = (string)jso["grades"];
                            // String decodeds = (string)jso["decoded"];
                            // Console.WriteLine("Decoded: " + decoded);
                            // Console.WriteLine("JSON  DATA : " + jsx);

                            var fileInfo = new
                            {
                                FileName = fileName,
                                FilePath = filePath,
                                jsonData = jsx,
                                decoded = decodedString
                            };


                            var json = JsonConvert.SerializeObject(fileInfo);
                            //var json = JsonConvert.SerializeObject(jso);
                            context.Response.ContentType = "application/json";

                            File.Delete(filePath);
                            File.Delete(filePath2);

                            await context.Response.WriteAsync(json, Encoding.UTF8);
                        }
                    }
                    else if (context.Request.Path == "/api/barcode")
                    {
                        Console.WriteLine("Called2");
                        // var file = context.Request.Form.Files[0];
                        // var fileSize = file.Length;
                        // Console.WriteLine("result : " + fileSize.ToString());
                        // await context.Response.WriteAsync(fileSize.ToString() + "some");

                        if (context.Request.Method == "POST")
                        {
                            var formCollection = await context.Request.ReadFormAsync();
                            var data = formCollection["data"];
                            // Convert the JSON string to a JObject
                            JObject jsonObject = JObject.Parse(data);
                            Console.WriteLine(jsonObject.ToString());

                            // Access individual items
                            Boolean rplane = (Boolean)jsonObject["rplane"];
                            float multi_factor = (float)jsonObject["multi_factor"];
                            Boolean contrast = (Boolean)jsonObject["contrast"];

                            // Display the extracted items
                            Console.WriteLine($"rplane: {rplane}");
                            Console.WriteLine($"multi_factor: {multi_factor}");
                            Console.WriteLine($"contrast: {contrast}");

                            var file = context.Request.Form.Files[0];
                            var fileName = Path.GetRandomFileName();
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName + ".png");

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            var fileSize = file.Length;
                            String jsx = GetBarCodeData(filePath, rplane, contrast, multi_factor);

                            //var jso = Action(filePath);
                            // String jsx = (string)jso["grades"];
                            // String decodeds = (string)jso["decoded"];
                            // Console.WriteLine("Decoded: " + decoded);
                            // Console.WriteLine("JSON  DATA : " + jsx);

                            var fileInfo = new
                            {
                                FileName = fileName,
                                FilePath = filePath,
                                decoded = barCodeString,
                                jsonData = jsx
                            };

                            //  Console.WriteLine(decodedString + ": DEDE");
                            // Console.WriteLine(fileInfo.ToString());

                            var json = JsonConvert.SerializeObject(fileInfo);
                            //var json = JsonConvert.SerializeObject(jso);
                            context.Response.ContentType = "application/json";

                            File.Delete(filePath);

                            await context.Response.WriteAsync(json, Encoding.UTF8);
                        }
                    }
                    else if (context.Request.Path == "/api/check")
                    {

                        await context.Response.WriteAsync("Server is on");
                    }
                    else if (context.Request.Path == "/api/filename")
                    {
                        var file = context.Request.Form.Files[0];
                        var fileName = file.FileName;
                        Console.WriteLine("result name : " + fileName.ToString());

                        await context.Response.WriteAsync("result name : " + fileName.ToString());
                    }
                });
            })
            .Build();

        await host.RunAsync();
    }

    public static string GetLocalIpAddress2()
    {
        string localIpAddress = null;

        try
        {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                    networkInterface.OperationalStatus != OperationalStatus.Up ||
                    networkInterface.Description.ToLowerInvariant().Contains("virtual"))
                {
                    continue;
                }

                IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();

                foreach (UnicastIPAddressInformation unicastAddress in ipProperties.UnicastAddresses)
                {
                    if (unicastAddress.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
                        !IPAddress.IsLoopback(unicastAddress.Address) &&
                        !IsLinkLocal(unicastAddress.Address))
                    {
                        localIpAddress = unicastAddress.Address.ToString();
                        break;
                    }
                }

                if (localIpAddress != null)
                {
                    break; // Found a valid IP address, break the loop
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting local IP address: {ex.Message}");
        }

        return localIpAddress;
    }

    private static bool IsLinkLocal(IPAddress address)
    {
        // Check if the address is in the range of link-local addresses (169.254.0.0 to 169.254.255.255)
        byte[] bytes = address.GetAddressBytes();
        return bytes[0] == 169 && bytes[1] == 254;
    }

    public static void grade_data_code_2d(HTuple hv_DataCodeHandle, HTuple hv_ResultHandle,
        HTuple hv_Standard, HTuple hv_Format, HTuple hv_Mode, out HTuple hv_GradingResults)
    {



        // Local iconic variables 

        HObject ho_Modules = null, ho_ModuleCenters = null;
        HObject ho_EmptyObject = null, ho_GradeModules = null;

        // Local control variables 

        HTuple hv_QualityStandards = new HTuple();
        HTuple hv_Message = new HTuple(), hv_GradeFormats = new HTuple();
        HTuple hv_GradingResultModes = new HTuple(), hv_ParamQualityStd = new HTuple();
        HTuple hv_ParamLabels = new HTuple(), hv_ResultParams = new HTuple();
        HTuple hv_QualityResultParams = new HTuple(), hv_SymbolType = new HTuple();
        HTuple hv_CodeTypes = new HTuple(), hv_ParamsAvailable = new HTuple();
        HTuple hv_IndexParamLabels = new HTuple(), hv_GradeNumbers = new HTuple();
        HTuple hv_GradeLetters = new HTuple(), hv_ParamGrades = new HTuple();
        HTuple hv_Labels = new HTuple(), hv_Grades = new HTuple();
        HTuple hv_Index = new HTuple(), hv_Grade = new HTuple();
        HTuple hv_ParamFloatGrades = new HTuple(), hv_FloatGrades = new HTuple();
        HTuple hv_ParamValues = new HTuple(), hv_Values = new HTuple();
        HTuple hv_GradesData = new HTuple(), hv_Data = new HTuple();
        HTuple hv_ParamAdditionalReflectanceCheck = new HTuple();
        HTuple hv_AdditionalReflectanceCheck = new HTuple(), hv_ParamIntermLabels = new HTuple();
        HTuple hv_ParamIntermGrades = new HTuple(), hv_ParamIntermValues = new HTuple();
        HTuple hv_IntermediateResults = new HTuple(), hv_IntermediateLabels = new HTuple();
        HTuple hv_IntermediateGrades = new HTuple(), hv_IntermediateValues = new HTuple();
        HTuple hv_IntermediateGrade = new HTuple(), hv_ModuleData = new HTuple();
        HTuple hv_ParamRows = new HTuple(), hv_ParamCols = new HTuple();
        HTuple hv_Rows = new HTuple(), hv_Cols = new HTuple();
        HTuple hv_Aperture = new HTuple(), hv_ModuleWidth = new HTuple();
        HTuple hv_ModuleHeight = new HTuple(), hv_Radius = new HTuple();
        HTuple hv_RadiusTup = new HTuple(), hv_ParamReflectanceMargin = new HTuple();
        HTuple hv_ReflectanceData = new HTuple(), hv_ReflectanceMarginModuleGrades = new HTuple();
        HTuple hv_GradeIndices = new HTuple(), hv_NameModuleGrade = new HTuple();
        HTuple hv_GradeRows = new HTuple(), hv_GradeCols = new HTuple();
        HTuple hv_GradeRadius = new HTuple(), hv_Keys = new HTuple();
        // Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(out ho_Modules);
        HOperatorSet.GenEmptyObj(out ho_ModuleCenters);
        HOperatorSet.GenEmptyObj(out ho_EmptyObject);
        HOperatorSet.GenEmptyObj(out ho_GradeModules);
        hv_GradingResults = new HTuple();
        try
        {
            //This procedure performs print quality inspection on
            //data codes and returns all results in a dictionary.
            //
            //The corresponding quality standard can be selected
            //with Standard. Quality grades can be returned as
            //numbers 0-4 or letters F-A (Format).
            //Mode defines if only the grades or all (intermediate)
            //quality grading results are returned.
            //
            //
            //First, check the input parameters.
            //
            //Parameter Standard.
            if ((int)(new HTuple((new HTuple(hv_Standard.TupleLength())).TupleNotEqual(
                1))) != 0)
            {
                throw new HalconException("Wrong number of values for the parameter Standard. Please specify exactly one quality standard.");
            }
            hv_QualityStandards.Dispose();
            hv_QualityStandards = new HTuple();
            hv_QualityStandards[0] = "isoiec15415";
            hv_QualityStandards[1] = "isoiec29158";
            hv_QualityStandards[2] = "aimdpm_1_2006";
            hv_QualityStandards[3] = "semi_t10";
            hv_QualityStandards[4] = "isoiec_tr_29158";
            if ((int)(new HTuple(((hv_QualityStandards.TupleFind(hv_Standard))).TupleEqual(
                -1))) != 0)
            {
                //The parameter 'isoiec_tr_29158' was replaced by 'isoiec29158' and
                //is only available for backward compatibility.
                hv_Message.Dispose();
                hv_Message = "Invalid quality standard. Possible values for Standard are: ";
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    {
                        HTuple
                          ExpTmpLocalVar_Message = (hv_Message + (((hv_QualityStandards.TupleSelectRange(
                            0, 3))).TupleJoin(new HTuple(", ")))) + ".";
                        hv_Message.Dispose();
                        hv_Message = ExpTmpLocalVar_Message;
                    }
                }
                throw new HalconException(hv_Message);
            }
            //Parameter Format.
            if ((int)(new HTuple((new HTuple(hv_Format.TupleLength())).TupleNotEqual(1))) != 0)
            {
                throw new HalconException("Wrong number of values for the parameter Format. Please specify exactly one grade format.");
            }
            hv_GradeFormats.Dispose();
            hv_GradeFormats = new HTuple();
            hv_GradeFormats[0] = "numeric";
            hv_GradeFormats[1] = "alphabetic";
            if ((int)(new HTuple(((hv_GradeFormats.TupleFind(hv_Format))).TupleEqual(-1))) != 0)
            {
                hv_Message.Dispose();
                hv_Message = "Invalid grade format. Possible values for Format are: ";
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    {
                        HTuple
                          ExpTmpLocalVar_Message = (hv_Message + (hv_GradeFormats.TupleJoin(
                            new HTuple(", ")))) + ".";
                        hv_Message.Dispose();
                        hv_Message = ExpTmpLocalVar_Message;
                    }
                }
                throw new HalconException(hv_Message);
            }
            if ((int)((new HTuple(hv_Standard.TupleEqual("semi_t10"))).TupleAnd(new HTuple(hv_Format.TupleEqual(
                "alphabetic")))) != 0)
            {
                throw new HalconException("Alphabetic grades are not supported for the selected quality standard. Please use the numeric format.");
            }
            //Parameter Mode.
            if ((int)(new HTuple((new HTuple(hv_Mode.TupleLength())).TupleNotEqual(1))) != 0)
            {
                throw new HalconException("Wrong number of values for the parameter Mode. Please specify exactly one grading result mode.");
            }
            hv_GradingResultModes.Dispose();
            hv_GradingResultModes = new HTuple();
            hv_GradingResultModes[0] = "grades";
            hv_GradingResultModes[1] = "all";
            if ((int)(new HTuple(((hv_GradingResultModes.TupleFind(hv_Mode))).TupleEqual(
                -1))) != 0)
            {
                hv_Message.Dispose();
                hv_Message = "Invalid grading result mode. Possible values for Mode are: ";
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    {
                        HTuple
                          ExpTmpLocalVar_Message = (hv_Message + (hv_GradingResultModes.TupleJoin(
                            new HTuple(", ")))) + ".";
                        hv_Message.Dispose();
                        hv_Message = ExpTmpLocalVar_Message;
                    }
                }
                throw new HalconException(hv_Message);
            }
            //Query the possible quality results for the given data code type.
            hv_ParamQualityStd.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_ParamQualityStd = "quality_" + hv_Standard;
            }
            hv_ParamLabels.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_ParamLabels = hv_ParamQualityStd + "_labels";
            }
            hv_ResultParams.Dispose();
            HOperatorSet.QueryDataCode2dParams(hv_DataCodeHandle, "get_result_params",
                out hv_ResultParams);
            hv_QualityResultParams.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_QualityResultParams = hv_ResultParams.TupleRegexpSelect(
                    "quality_");
            }
            if ((int)(new HTuple(hv_Standard.TupleEqual("isoiec_tr_29158"))) != 0)
            {
                //The parameters 'quality_isoiec_tr_29158_...' were replaced by the parameters
                //'quality_isoiec29158_...' and are only available for backward compatibility.
                hv_SymbolType.Dispose();
                HOperatorSet.GetDataCode2dParam(hv_DataCodeHandle, "symbol_type", out hv_SymbolType);
                hv_CodeTypes.Dispose();
                hv_CodeTypes = new HTuple();
                hv_CodeTypes[0] = "Data Matrix ECC 200";
                hv_CodeTypes[1] = "GS1 DataMatrix";
                hv_CodeTypes[2] = "QR Code";
                hv_CodeTypes[3] = "Micro QR Code";
                hv_CodeTypes[4] = "GS1 QR Code";
                hv_CodeTypes[5] = "Aztec Code";
                hv_CodeTypes[6] = "GS1 Aztec Code";
                hv_ParamsAvailable.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_ParamsAvailable = new HTuple(((hv_CodeTypes.TupleFind(
                        hv_SymbolType))).TupleGreaterEqual(0));
                }
                if ((int)(hv_ParamsAvailable) != 0)
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_QualityResultParams = hv_QualityResultParams.TupleConcat(
                                ((new HTuple("quality_isoiec_tr_29158")).TupleConcat("quality_isoiec_tr_29158_labels")).TupleConcat(
                                "quality_isoiec_tr_29158_values"));
                            hv_QualityResultParams.Dispose();
                            hv_QualityResultParams = ExpTmpLocalVar_QualityResultParams;
                        }
                    }
                    if ((int)((new HTuple(hv_SymbolType.TupleEqual("Data Matrix ECC 200"))).TupleOr(
                        new HTuple(hv_SymbolType.TupleEqual("GS1 DataMatrix")))) != 0)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            {
                                HTuple
                                  ExpTmpLocalVar_QualityResultParams = hv_QualityResultParams.TupleConcat(
                                    (((((new HTuple("quality_isoiec_tr_29158_intermediate")).TupleConcat(
                                    "quality_isoiec_tr_29158_intermediate_labels")).TupleConcat("quality_isoiec_tr_29158_intermediate_values")).TupleConcat(
                                    "quality_isoiec_tr_29158_rows")).TupleConcat("quality_isoiec_tr_29158_cols")).TupleConcat(
                                    "quality_isoiec_tr_29158_reflectance_margin_module_grades"));
                                hv_QualityResultParams.Dispose();
                                hv_QualityResultParams = ExpTmpLocalVar_QualityResultParams;
                            }
                        }
                    }
                }
            }
            //
            hv_IndexParamLabels.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_IndexParamLabels = hv_QualityResultParams.TupleFind(
                    hv_ParamLabels);
            }
            if ((int)((new HTuple(hv_IndexParamLabels.TupleEqual(-1))).TupleOr(new HTuple(hv_IndexParamLabels.TupleEqual(
                new HTuple())))) != 0)
            {
                hv_Message.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Message = ("The quality standard " + hv_Standard) + " is not supported for this code type.";
                }
                throw new HalconException(hv_Message);
            }
            //
            //Now, perform quality grading and add results to output dictionary.
            //
            hv_GradingResults.Dispose();
            HOperatorSet.CreateDict(out hv_GradingResults);
            hv_GradeNumbers.Dispose();
            hv_GradeNumbers = new HTuple();
            hv_GradeNumbers[0] = 0;
            hv_GradeNumbers[1] = 1;
            hv_GradeNumbers[2] = 2;
            hv_GradeNumbers[3] = 3;
            hv_GradeNumbers[4] = 4;
            hv_GradeLetters.Dispose();
            hv_GradeLetters = new HTuple();
            hv_GradeLetters[0] = "F";
            hv_GradeLetters[1] = "D";
            hv_GradeLetters[2] = "C";
            hv_GradeLetters[3] = "B";
            hv_GradeLetters[4] = "A";
            if ((int)(new HTuple(hv_Standard.TupleEqual("semi_t10"))) != 0)
            {
                //For the SEMI T10 standard no grades but values are returned.
                hv_ParamGrades.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_ParamGrades = hv_ParamQualityStd + "_values";
                }
            }
            else
            {
                hv_ParamGrades.Dispose();
                hv_ParamGrades = new HTuple(hv_ParamQualityStd);
            }
            //
            hv_Labels.Dispose();
            HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandle, hv_ParamLabels,
                out hv_Labels);
            hv_Grades.Dispose();
            HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandle, hv_ParamGrades,
                out hv_Grades);
            if ((int)((new HTuple((new HTuple(hv_Mode.TupleEqual("grades"))).TupleAnd(new HTuple(hv_Format.TupleEqual(
                "numeric"))))).TupleOr(new HTuple(hv_Standard.TupleEqual("semi_t10")))) != 0)
            {
                //Return labels and corresponding grades in numbers (0 to 4).
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Labels.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        HOperatorSet.SetDictTuple(hv_GradingResults, hv_Labels.TupleSelect(hv_Index),
                            hv_Grades.TupleSelect(hv_Index));
                    }
                }
            }
            else if ((int)((new HTuple(hv_Mode.TupleEqual("grades"))).TupleAnd(
                new HTuple(hv_Format.TupleEqual("alphabetic")))) != 0)
            {
                //Return labels and corresponding grades in letters (F to A).
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Labels.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    if ((int)(((((hv_Grades.TupleSelect(hv_Index))).TupleIsInt())).TupleAnd(
                        new HTuple(((hv_GradeNumbers.TupleFind(hv_Grades.TupleSelect(hv_Index)))).TupleGreaterEqual(
                        0)))) != 0)
                    {
                        hv_Grade.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_Grade = hv_GradeLetters.TupleSelect(
                                hv_Grades.TupleSelect(hv_Index));
                        }
                    }
                    else
                    {
                        hv_Grade.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_Grade = hv_Grades.TupleSelect(
                                hv_Index);
                        }
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        HOperatorSet.SetDictTuple(hv_GradingResults, hv_Labels.TupleSelect(hv_Index),
                            hv_Grade);
                    }
                }
            }
            else if ((int)(new HTuple(hv_Mode.TupleEqual("all"))) != 0)
            {
                //Return labels and corresponding grades in numbers or letters
                //as well as the raw values for all directly measurable grades.
                hv_ParamFloatGrades.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_ParamFloatGrades = hv_ParamQualityStd + "_float_grades";
                }
                if ((int)(new HTuple(((hv_QualityResultParams.TupleFind(hv_ParamFloatGrades))).TupleGreaterEqual(
                    0))) != 0)
                {
                    hv_FloatGrades.Dispose();
                    HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandle, hv_ParamFloatGrades,
                        out hv_FloatGrades);
                }
                else
                {
                    hv_FloatGrades.Dispose();
                    hv_FloatGrades = new HTuple();
                }
                hv_ParamValues.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_ParamValues = hv_ParamQualityStd + "_values";
                }
                hv_Values.Dispose();
                HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandle, hv_ParamValues,
                    out hv_Values);
                hv_GradesData.Dispose();
                HOperatorSet.CreateDict(out hv_GradesData);
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Labels.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    hv_Data.Dispose();
                    HOperatorSet.CreateDict(out hv_Data);
                    if ((int)((new HTuple((new HTuple(hv_Format.TupleEqual("alphabetic"))).TupleAnd(
                        ((hv_Grades.TupleSelect(hv_Index))).TupleIsInt()))).TupleAnd(new HTuple(((hv_GradeNumbers.TupleFind(
                        hv_Grades.TupleSelect(hv_Index)))).TupleGreaterEqual(0)))) != 0)
                    {
                        hv_Grade.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_Grade = hv_GradeLetters.TupleSelect(
                                hv_Grades.TupleSelect(hv_Index));
                        }
                    }
                    else
                    {
                        hv_Grade.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_Grade = hv_Grades.TupleSelect(
                                hv_Index);
                        }
                    }
                    HOperatorSet.SetDictTuple(hv_Data, "Grade", hv_Grade);
                    if ((int)(new HTuple(hv_FloatGrades.TupleNotEqual(new HTuple()))) != 0)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            HOperatorSet.SetDictTuple(hv_Data, "Grade (float)", hv_FloatGrades.TupleSelect(
                                hv_Index));
                        }
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        HOperatorSet.SetDictTuple(hv_Data, "Value", hv_Values.TupleSelect(hv_Index));
                    }
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        HOperatorSet.SetDictTuple(hv_GradesData, hv_Labels.TupleSelect(hv_Index),
                            hv_Data);
                    }
                }
                HOperatorSet.SetDictTuple(hv_GradingResults, "Grades", hv_GradesData);
            }
            //
            if ((int)(new HTuple(hv_Mode.TupleEqual("all"))) != 0)
            {
                //Additional reflectance check (if available).
                hv_ParamAdditionalReflectanceCheck.Dispose();
                hv_ParamAdditionalReflectanceCheck = "quality_isoiec15415_additional_reflectance_check";
                if ((int)(new HTuple(hv_Standard.TupleEqual("isoiec15415"))) != 0)
                {
                    if ((int)(new HTuple(((hv_QualityResultParams.TupleFind(hv_ParamAdditionalReflectanceCheck))).TupleGreaterEqual(
                        0))) != 0)
                    {
                        hv_AdditionalReflectanceCheck.Dispose();
                        HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandle,
                            hv_ParamAdditionalReflectanceCheck, out hv_AdditionalReflectanceCheck);
                        HOperatorSet.SetDictTuple(hv_GradingResults, "Additional Reflectance Check",
                            hv_AdditionalReflectanceCheck);
                    }
                }
                //
                //Intermediate grading results (if available).
                hv_ParamIntermLabels.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_ParamIntermLabels = hv_ParamQualityStd + "_intermediate_labels";
                }
                hv_ParamIntermGrades.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_ParamIntermGrades = hv_ParamQualityStd + "_intermediate";
                }
                hv_ParamIntermValues.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_ParamIntermValues = hv_ParamQualityStd + "_intermediate_values";
                }
                if ((int)(new HTuple(((hv_QualityResultParams.TupleFind(hv_ParamIntermLabels))).TupleGreaterEqual(
                    0))) != 0)
                {
                    hv_IntermediateResults.Dispose();
                    HOperatorSet.CreateDict(out hv_IntermediateResults);
                    hv_IntermediateLabels.Dispose();
                    HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandle, hv_ParamIntermLabels,
                        out hv_IntermediateLabels);
                    hv_IntermediateGrades.Dispose();
                    HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandle, hv_ParamIntermGrades,
                        out hv_IntermediateGrades);
                    hv_IntermediateValues.Dispose();
                    HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandle, hv_ParamIntermValues,
                        out hv_IntermediateValues);
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_IntermediateLabels.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        hv_Data.Dispose();
                        HOperatorSet.CreateDict(out hv_Data);
                        if ((int)((new HTuple((new HTuple(hv_Format.TupleEqual("alphabetic"))).TupleAnd(
                            ((hv_IntermediateGrades.TupleSelect(hv_Index))).TupleIsInt()))).TupleAnd(
                            new HTuple(((hv_GradeNumbers.TupleFind(hv_IntermediateGrades.TupleSelect(
                            hv_Index)))).TupleGreaterEqual(0)))) != 0)
                        {
                            hv_IntermediateGrade.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_IntermediateGrade = hv_GradeLetters.TupleSelect(
                                    hv_IntermediateGrades.TupleSelect(hv_Index));
                            }
                        }
                        else
                        {
                            hv_IntermediateGrade.Dispose();
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_IntermediateGrade = hv_IntermediateGrades.TupleSelect(
                                    hv_Index);
                            }
                        }
                        HOperatorSet.SetDictTuple(hv_Data, "Grade", hv_IntermediateGrade);
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            HOperatorSet.SetDictTuple(hv_Data, "Value", hv_IntermediateValues.TupleSelect(
                                hv_Index));
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            HOperatorSet.SetDictTuple(hv_IntermediateResults, hv_IntermediateLabels.TupleSelect(
                                hv_Index), hv_Data);
                        }
                    }
                    HOperatorSet.SetDictTuple(hv_GradingResults, "Intermediate Results", hv_IntermediateResults);
                }
                //
                //Module Data (if available).
                hv_ModuleData.Dispose();
                HOperatorSet.CreateDict(out hv_ModuleData);
                hv_ParamRows.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_ParamRows = hv_ParamQualityStd + "_rows";
                }
                hv_ParamCols.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_ParamCols = hv_ParamQualityStd + "_cols";
                }
                if ((int)(new HTuple(((hv_QualityResultParams.TupleFind(hv_ParamRows))).TupleGreaterEqual(
                    0))) != 0)
                {
                    hv_Rows.Dispose();
                    HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandle, hv_ParamRows,
                        out hv_Rows);
                    hv_Cols.Dispose();
                    HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandle, hv_ParamCols,
                        out hv_Cols);
                    HOperatorSet.SetDictTuple(hv_ModuleData, "Rows", hv_Rows);
                    HOperatorSet.SetDictTuple(hv_ModuleData, "Cols", hv_Cols);
                    if ((int)(new HTuple((new HTuple(hv_Rows.TupleLength())).TupleGreater(0))) != 0)
                    {
                        //Create module rois and centers using aperture and module size.
                        hv_Aperture.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_Aperture = hv_Grades.TupleSelect(
                                hv_Labels.TupleFind("Aperture"));
                        }
                        hv_ModuleWidth.Dispose();
                        HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandle,
                            "module_width", out hv_ModuleWidth);
                        hv_ModuleHeight.Dispose();
                        HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandle,
                            "module_height", out hv_ModuleHeight);
                        hv_Radius.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_Radius = (0.5 * hv_Aperture) * (hv_ModuleWidth.TupleMin2(
                                hv_ModuleHeight));
                        }
                        hv_RadiusTup.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_RadiusTup = HTuple.TupleGenConst(
                                new HTuple(hv_Rows.TupleLength()), hv_Radius);
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            ho_Modules.Dispose();
                            HOperatorSet.GenCircleContourXld(out ho_Modules, hv_Rows, hv_Cols, hv_RadiusTup,
                                (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad(), "positive",
                                1);
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            ho_ModuleCenters.Dispose();
                            HOperatorSet.GenCrossContourXld(out ho_ModuleCenters, hv_Rows, hv_Cols,
                                2 * hv_Radius, (new HTuple(0)).TupleRad());
                        }
                    }
                    else
                    {
                        ho_Modules.Dispose();
                        HOperatorSet.GenEmptyObj(out ho_Modules);
                        ho_ModuleCenters.Dispose();
                        HOperatorSet.GenEmptyObj(out ho_ModuleCenters);
                    }
                    HOperatorSet.SetDictObject(ho_Modules, hv_ModuleData, "Rois");
                    HOperatorSet.SetDictObject(ho_ModuleCenters, hv_ModuleData, "Centers");
                }
                //Get reflectance margin module grades.
                hv_ParamReflectanceMargin.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_ParamReflectanceMargin = hv_ParamQualityStd + "_reflectance_margin_module_grades";
                }
                if ((int)(new HTuple(((hv_QualityResultParams.TupleFind(hv_ParamReflectanceMargin))).TupleNotEqual(
                    -1))) != 0)
                {
                    hv_ReflectanceData.Dispose();
                    HOperatorSet.CreateDict(out hv_ReflectanceData);
                    hv_ReflectanceMarginModuleGrades.Dispose();
                    HOperatorSet.GetDataCode2dResults(hv_DataCodeHandle, hv_ResultHandle, hv_ParamReflectanceMargin,
                        out hv_ReflectanceMarginModuleGrades);
                    HOperatorSet.SetDictTuple(hv_ReflectanceData, "Module Grades", hv_ReflectanceMarginModuleGrades);
                    for (hv_Grade = 0; (int)hv_Grade <= 4; hv_Grade = (int)hv_Grade + 1)
                    {
                        hv_GradeIndices.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_GradeIndices = hv_ReflectanceMarginModuleGrades.TupleFind(
                                hv_Grade);
                        }
                        hv_NameModuleGrade.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_NameModuleGrade = "Modules Grade " + hv_Grade;
                        }
                        if ((int)((new HTuple(hv_GradeIndices.TupleEqual(-1))).TupleOr(new HTuple(hv_Rows.TupleEqual(
                            new HTuple())))) != 0)
                        {
                            ho_EmptyObject.Dispose();
                            HOperatorSet.GenEmptyObj(out ho_EmptyObject);
                            HOperatorSet.SetDictObject(ho_EmptyObject, hv_ReflectanceData, hv_NameModuleGrade);
                            continue;
                        }
                        hv_GradeRows.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_GradeRows = hv_Rows.TupleSelect(
                                hv_GradeIndices);
                        }
                        hv_GradeCols.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_GradeCols = hv_Cols.TupleSelect(
                                hv_GradeIndices);
                        }
                        hv_GradeRadius.Dispose();
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_GradeRadius = HTuple.TupleGenConst(
                                new HTuple(hv_GradeRows.TupleLength()), hv_Radius);
                        }
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            ho_GradeModules.Dispose();
                            HOperatorSet.GenCircleContourXld(out ho_GradeModules, hv_GradeRows, hv_GradeCols,
                                hv_GradeRadius, (new HTuple(0)).TupleRad(), (new HTuple(360)).TupleRad()
                                , "positive", 1);
                        }
                        HOperatorSet.SetDictObject(ho_GradeModules, hv_ReflectanceData, hv_NameModuleGrade);
                    }
                    HOperatorSet.SetDictTuple(hv_ModuleData, "Reflectance Margin", hv_ReflectanceData);
                }
                hv_Keys.Dispose();
                HOperatorSet.GetDictParam(hv_ModuleData, "keys", new HTuple(), out hv_Keys);
                if ((int)(new HTuple(hv_Keys.TupleNotEqual(new HTuple()))) != 0)
                {
                    HOperatorSet.SetDictTuple(hv_GradingResults, "Modules", hv_ModuleData);
                }
            }
            //
            ho_Modules.Dispose();
            ho_ModuleCenters.Dispose();
            ho_EmptyObject.Dispose();
            ho_GradeModules.Dispose();

            hv_QualityStandards.Dispose();
            hv_Message.Dispose();
            hv_GradeFormats.Dispose();
            hv_GradingResultModes.Dispose();
            hv_ParamQualityStd.Dispose();
            hv_ParamLabels.Dispose();
            hv_ResultParams.Dispose();
            hv_QualityResultParams.Dispose();
            hv_SymbolType.Dispose();
            hv_CodeTypes.Dispose();
            hv_ParamsAvailable.Dispose();
            hv_IndexParamLabels.Dispose();
            hv_GradeNumbers.Dispose();
            hv_GradeLetters.Dispose();
            hv_ParamGrades.Dispose();
            hv_Labels.Dispose();
            hv_Grades.Dispose();
            hv_Index.Dispose();
            hv_Grade.Dispose();
            hv_ParamFloatGrades.Dispose();
            hv_FloatGrades.Dispose();
            hv_ParamValues.Dispose();
            hv_Values.Dispose();
            hv_GradesData.Dispose();
            hv_Data.Dispose();
            hv_ParamAdditionalReflectanceCheck.Dispose();
            hv_AdditionalReflectanceCheck.Dispose();
            hv_ParamIntermLabels.Dispose();
            hv_ParamIntermGrades.Dispose();
            hv_ParamIntermValues.Dispose();
            hv_IntermediateResults.Dispose();
            hv_IntermediateLabels.Dispose();
            hv_IntermediateGrades.Dispose();
            hv_IntermediateValues.Dispose();
            hv_IntermediateGrade.Dispose();
            hv_ModuleData.Dispose();
            hv_ParamRows.Dispose();
            hv_ParamCols.Dispose();
            hv_Rows.Dispose();
            hv_Cols.Dispose();
            hv_Aperture.Dispose();
            hv_ModuleWidth.Dispose();
            hv_ModuleHeight.Dispose();
            hv_Radius.Dispose();
            hv_RadiusTup.Dispose();
            hv_ParamReflectanceMargin.Dispose();
            hv_ReflectanceData.Dispose();
            hv_ReflectanceMarginModuleGrades.Dispose();
            hv_GradeIndices.Dispose();
            hv_NameModuleGrade.Dispose();
            hv_GradeRows.Dispose();
            hv_GradeCols.Dispose();
            hv_GradeRadius.Dispose();
            hv_Keys.Dispose();

            return;
        }
        catch (HalconException HDevExpDefaultException)
        {
            ho_Modules.Dispose();
            ho_ModuleCenters.Dispose();
            ho_EmptyObject.Dispose();
            ho_GradeModules.Dispose();

            hv_QualityStandards.Dispose();
            hv_Message.Dispose();
            hv_GradeFormats.Dispose();
            hv_GradingResultModes.Dispose();
            hv_ParamQualityStd.Dispose();
            hv_ParamLabels.Dispose();
            hv_ResultParams.Dispose();
            hv_QualityResultParams.Dispose();
            hv_SymbolType.Dispose();
            hv_CodeTypes.Dispose();
            hv_ParamsAvailable.Dispose();
            hv_IndexParamLabels.Dispose();
            hv_GradeNumbers.Dispose();
            hv_GradeLetters.Dispose();
            hv_ParamGrades.Dispose();
            hv_Labels.Dispose();
            hv_Grades.Dispose();
            hv_Index.Dispose();
            hv_Grade.Dispose();
            hv_ParamFloatGrades.Dispose();
            hv_FloatGrades.Dispose();
            hv_ParamValues.Dispose();
            hv_Values.Dispose();
            hv_GradesData.Dispose();
            hv_Data.Dispose();
            hv_ParamAdditionalReflectanceCheck.Dispose();
            hv_AdditionalReflectanceCheck.Dispose();
            hv_ParamIntermLabels.Dispose();
            hv_ParamIntermGrades.Dispose();
            hv_ParamIntermValues.Dispose();
            hv_IntermediateResults.Dispose();
            hv_IntermediateLabels.Dispose();
            hv_IntermediateGrades.Dispose();
            hv_IntermediateValues.Dispose();
            hv_IntermediateGrade.Dispose();
            hv_ModuleData.Dispose();
            hv_ParamRows.Dispose();
            hv_ParamCols.Dispose();
            hv_Rows.Dispose();
            hv_Cols.Dispose();
            hv_Aperture.Dispose();
            hv_ModuleWidth.Dispose();
            hv_ModuleHeight.Dispose();
            hv_Radius.Dispose();
            hv_RadiusTup.Dispose();
            hv_ParamReflectanceMargin.Dispose();
            hv_ReflectanceData.Dispose();
            hv_ReflectanceMarginModuleGrades.Dispose();
            hv_GradeIndices.Dispose();
            hv_NameModuleGrade.Dispose();
            hv_GradeRows.Dispose();
            hv_GradeCols.Dispose();
            hv_GradeRadius.Dispose();
            hv_Keys.Dispose();

            throw HDevExpDefaultException;
        }
    }

    // Main procedure 
    public static String Action(String filepath, Boolean isRplane, Boolean useContrast)
    {
        // Local iconic variables 
        //   Console.WriteLine("HERE");

        HObject ho_Mat, ho_SymbolXLDs, rPlane, ho_ImageEmphasize, ho_gray;
        HTuple Width, Height;

        // Local control variables 

        HTuple hv_DataCodeHandle = new HTuple(), hv_ResultHandles = new HTuple();
        HTuple hv_DecodedDataStrings = new HTuple(), hv_GradingResults = new HTuple();
        HTuple hv_JsonString = new HTuple();
        // Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(out ho_Mat);
        HOperatorSet.GenEmptyObj(out ho_SymbolXLDs);
        try
        {
            //  Console.WriteLine("DEDE");
            ho_Mat.Dispose();
            HOperatorSet.ReadImage(out ho_Mat, filepath);

            hv_DataCodeHandle.Dispose();
            HOperatorSet.CreateDataCode2dModel("Data Matrix ECC 200", new HTuple(), new HTuple(),
                out hv_DataCodeHandle);
            //  Console.WriteLine("DEDE2");

            ho_SymbolXLDs.Dispose(); hv_ResultHandles.Dispose(); hv_DecodedDataStrings.Dispose();
            if (isRplane)
            {
                HOperatorSet.Decompose3(ho_Mat, out _, out _, out ho_gray);
            }
            else
            {
                HOperatorSet.Rgb1ToGray(ho_Mat, out ho_gray);
            }
            if (useContrast)
            {
                HOperatorSet.GetImageSize(ho_gray, out Width, out Height);
                HOperatorSet.Emphasize(ho_gray, out ho_ImageEmphasize, Width, Height, 2);
                HOperatorSet.FindDataCode2d(ho_ImageEmphasize, out ho_SymbolXLDs, hv_DataCodeHandle, new HTuple(),
                    new HTuple(), out hv_ResultHandles, out hv_DecodedDataStrings);
            }
            else
            {
                HOperatorSet.FindDataCode2d(ho_gray, out ho_SymbolXLDs, hv_DataCodeHandle, new HTuple(),
                new HTuple(), out hv_ResultHandles, out hv_DecodedDataStrings);
            }


            // Console.WriteLine("DED33E");
            decodedString = hv_DecodedDataStrings.ToString();
            Console.WriteLine("decoded: " + hv_DecodedDataStrings.ToString());


            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_GradingResults.Dispose();
                grade_data_code_2d(hv_DataCodeHandle, hv_ResultHandles, "isoiec15415",
                    "numeric", "grades", out hv_GradingResults);
            }

            hv_JsonString.Dispose();
            HOperatorSet.DictToJson(hv_GradingResults, new HTuple(), new HTuple(), out hv_JsonString);
            //  Console.WriteLine("JSN: " + hv_JsonString.ToString());
            // Console.WriteLine("DEDE55");
            //    Console.WriteLine("DEDE44 " + hv_JsonString.ToString());


            // var jso = new
            // {
            //     decoded = hv_DecodedDataStrings.ToString(),
            //     jsonData = hv_JsonString.ToString()
            // };
            // return jso;
            // decodedString = hv_DecodedDataStrings.ToString();

            return hv_JsonString;
        }
        catch (HalconException HDevExpDefaultException)
        {
            ho_Mat.Dispose();
            ho_SymbolXLDs.Dispose();

            hv_DataCodeHandle.Dispose();
            hv_ResultHandles.Dispose();
            hv_DecodedDataStrings.Dispose();
            hv_GradingResults.Dispose();
            hv_JsonString.Dispose();

            //  throw HDevExpDefaultException;
            Console.WriteLine(HDevExpDefaultException.ToString());

            decodedString = "";
            // var jso = new
            // {
            //     decoded = "NA",
            //     jsonData = "NA"
            // };
            // return jso;
            return "NA";
        }
        ho_Mat.Dispose();
        ho_SymbolXLDs.Dispose();

        hv_DataCodeHandle.Dispose();
        hv_ResultHandles.Dispose();
        hv_DecodedDataStrings.Dispose();
        hv_GradingResults.Dispose();
        hv_JsonString.Dispose();

    }

    public static String ActionNorm(String filewhite, String filepath2, Boolean isRplane, Boolean useContrast, float multi_factor)
    {
        // Local iconic variables 
        //   Console.WriteLine("HERE");

        HObject ho_Mat, ho_SymbolXLDs, rPlane, ho_ImageEmphasize;
        HTuple Width, Height;

        // Local control variables 
        HTuple h1, h2, w1, w2;
        HObject mat1, mat2, mat3, gray1, gray2, invert1;


        HTuple hv_DataCodeHandle = new HTuple(), hv_ResultHandles = new HTuple();
        HTuple hv_DecodedDataStrings = new HTuple(), hv_GradingResults = new HTuple();
        HTuple hv_JsonString = new HTuple();
        // Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(out ho_Mat);
        HOperatorSet.GenEmptyObj(out ho_SymbolXLDs);
        try
        {
            //  Console.WriteLine("DEDE");
            ho_Mat.Dispose();
            //  HOperatorSet.ReadImage(out ho_Mat, filepath);
            HOperatorSet.ReadImage(out mat1, filewhite);
            HOperatorSet.ReadImage(out mat2, filepath2);
            if (isRplane)
            {
                HOperatorSet.Decompose3(mat1, out _, out _, out gray1);
                HOperatorSet.Decompose3(mat2, out _, out _, out gray2);
            }
            else
            {
                HOperatorSet.Rgb1ToGray(mat1, out gray1);
                HOperatorSet.Rgb1ToGray(mat2, out gray2);
            }
            HOperatorSet.InvertImage(gray1, out invert1);
            HOperatorSet.MultImage(invert1, gray2, out ho_Mat, multi_factor, 0);

            hv_DataCodeHandle.Dispose();
            HOperatorSet.CreateDataCode2dModel("Data Matrix ECC 200", new HTuple(), new HTuple(),
                out hv_DataCodeHandle);
            //  Console.WriteLine("DEDE2");

            ho_SymbolXLDs.Dispose(); hv_ResultHandles.Dispose(); hv_DecodedDataStrings.Dispose();
            if (useContrast)
            {
                HOperatorSet.GetImageSize(ho_Mat, out Width, out Height);
                HOperatorSet.Emphasize(ho_Mat, out ho_ImageEmphasize, Width, Height, 2);
                HOperatorSet.FindDataCode2d(ho_ImageEmphasize, out ho_SymbolXLDs, hv_DataCodeHandle, new HTuple(),
                    new HTuple(), out hv_ResultHandles, out hv_DecodedDataStrings);

            }
            else
            {
                HOperatorSet.FindDataCode2d(ho_Mat, out ho_SymbolXLDs, hv_DataCodeHandle, new HTuple(),
                    new HTuple(), out hv_ResultHandles, out hv_DecodedDataStrings);

            }

            // Console.WriteLine("DED33E");
            decodedString = hv_DecodedDataStrings.ToString();
            Console.WriteLine("decoded: " + hv_DecodedDataStrings.ToString());

            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_GradingResults.Dispose();
                grade_data_code_2d(hv_DataCodeHandle, hv_ResultHandles, "isoiec15415",
                    "numeric", "grades", out hv_GradingResults);
            }

            hv_JsonString.Dispose();
            HOperatorSet.DictToJson(hv_GradingResults, new HTuple(), new HTuple(), out hv_JsonString);
            //  Console.WriteLine("JSN: " + hv_JsonString.ToString());
            // Console.WriteLine("DEDE55");
            //    Console.WriteLine("DEDE44 " + hv_JsonString.ToString());


            // var jso = new
            // {
            //     decoded = hv_DecodedDataStrings.ToString(),
            //     jsonData = hv_JsonString.ToString()
            // };
            // return jso;
            // decodedString = hv_DecodedDataStrings.ToString();

            return hv_JsonString;
        }
        catch (HalconException HDevExpDefaultException)
        {
            ho_Mat.Dispose();
            ho_SymbolXLDs.Dispose();

            hv_DataCodeHandle.Dispose();
            hv_ResultHandles.Dispose();
            hv_DecodedDataStrings.Dispose();
            hv_GradingResults.Dispose();
            hv_JsonString.Dispose();

            //  throw HDevExpDefaultException;
            Console.WriteLine(HDevExpDefaultException.ToString());

            decodedString = "";
            // var jso = new
            // {
            //     decoded = "NA",
            //     jsonData = "NA"
            // };
            // return jso;
            return "NA";
        }
        ho_Mat.Dispose();
        ho_SymbolXLDs.Dispose();

        hv_DataCodeHandle.Dispose();
        hv_ResultHandles.Dispose();
        hv_DecodedDataStrings.Dispose();
        hv_GradingResults.Dispose();
        hv_JsonString.Dispose();

    }


    public static String GetBarCodeData(String filepath, Boolean isRplane, Boolean useContrast, float multi_factor)
    {

        Console.WriteLine("caeed");

        // Local iconic variables 

        HObject ho_Image, ho_SymbolRegions, ho_gray, ho_emph;

        HTuple Width, Height;

        // Local control variables 

        HTuple hv_BarCodeHandle = new HTuple(), hv_DecodedDataStrings = new HTuple();
        HTuple hv_Labels = new HTuple(), hv_Grades = new HTuple();
        // Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(out ho_Image);
        HOperatorSet.GenEmptyObj(out ho_SymbolRegions);
        ho_Image.Dispose();
        HOperatorSet.ReadImage(out ho_Image, filepath);

        hv_BarCodeHandle.Dispose();
        HOperatorSet.CreateBarCodeModel(new HTuple(), new HTuple(), out hv_BarCodeHandle);
        HOperatorSet.SetBarCodeParam(hv_BarCodeHandle, "persistence", 1);
        ho_SymbolRegions.Dispose(); hv_DecodedDataStrings.Dispose();
        if (isRplane)
        {
            HOperatorSet.Decompose3(ho_Image, out _, out _, out ho_gray);
            if (useContrast)
            {
                HOperatorSet.GetImageSize(ho_gray, out Width, out Height);
                HOperatorSet.Emphasize(ho_gray, out ho_emph, Width, Height, 1);
                HOperatorSet.FindBarCode(ho_emph, out ho_SymbolRegions, hv_BarCodeHandle, "auto",
                            out hv_DecodedDataStrings);

            }
            else
            {
                HOperatorSet.FindBarCode(ho_gray, out ho_SymbolRegions, hv_BarCodeHandle, "auto",
            out hv_DecodedDataStrings);
            }
        }
        else
        {
            HOperatorSet.Rgb1ToGray(ho_Image, out ho_gray);
            if (useContrast)
            {
                HOperatorSet.GetImageSize(ho_gray, out Width, out Height);
                HOperatorSet.Emphasize(ho_gray, out ho_emph, Width, Height, 1);
                HOperatorSet.FindBarCode(ho_emph, out ho_SymbolRegions, hv_BarCodeHandle, "auto",
                            out hv_DecodedDataStrings);
            }
            else
            {
                HOperatorSet.FindBarCode(ho_gray, out ho_SymbolRegions, hv_BarCodeHandle, "auto",
                            out hv_DecodedDataStrings);
            }

        }

        Console.WriteLine("Decoded" + hv_DecodedDataStrings.ToString());
        barCodeString = hv_DecodedDataStrings.ToString();

        hv_Labels.Dispose();
        HOperatorSet.GetBarCodeResult(hv_BarCodeHandle, 0, "quality_isoiec15416_labels",
            out hv_Labels);
        hv_Grades.Dispose();
        HOperatorSet.GetBarCodeResult(hv_BarCodeHandle, 0, "quality_isoiec15416_float_grades",
            out hv_Grades);

        return hv_Grades.ToString();


        ho_Image.Dispose();
        ho_SymbolRegions.Dispose();

        hv_BarCodeHandle.Dispose();
        hv_DecodedDataStrings.Dispose();
        hv_Labels.Dispose();
        hv_Grades.Dispose();

    }

    // Main procedure 
    private static void action()
    {


        // Local iconic variables 

        HObject ho_Image, ho_SymbolXLDs;

        // Local control variables 

        HTuple hv_DataCodeHandle = new HTuple(), hv_ResultHandles = new HTuple();
        HTuple hv_DecodedDataStrings = new HTuple(), hv_GradingResults = new HTuple();
        HTuple hv_JsonString = new HTuple();
        // Initialize local and output iconic variables 
        HOperatorSet.GenEmptyObj(out ho_Image);
        HOperatorSet.GenEmptyObj(out ho_SymbolXLDs);
        try
        {
            ho_Image.Dispose();
            HOperatorSet.ReadImage(out ho_Image, "C:/Users/DELL/Desktop/Codes/temp7.png");
            hv_DataCodeHandle.Dispose();
            HOperatorSet.CreateDataCode2dModel("Data Matrix ECC 200", new HTuple(), new HTuple(),
                out hv_DataCodeHandle);
            ho_SymbolXLDs.Dispose(); hv_ResultHandles.Dispose(); hv_DecodedDataStrings.Dispose();
            HOperatorSet.FindDataCode2d(ho_Image, out ho_SymbolXLDs, hv_DataCodeHandle,
                new HTuple(), new HTuple(), out hv_ResultHandles, out hv_DecodedDataStrings);
            hv_GradingResults.Dispose();
            grade_data_code_2d(hv_DataCodeHandle, hv_ResultHandles, "isoiec15415", "numeric",
                "grades", out hv_GradingResults);
            hv_JsonString.Dispose();
            HOperatorSet.DictToJson(hv_GradingResults, new HTuple(), new HTuple(), out hv_JsonString);
        }
        catch (HalconException HDevExpDefaultException)
        {
            ho_Image.Dispose();
            ho_SymbolXLDs.Dispose();

            hv_DataCodeHandle.Dispose();
            hv_ResultHandles.Dispose();
            hv_DecodedDataStrings.Dispose();
            hv_GradingResults.Dispose();
            hv_JsonString.Dispose();

            throw HDevExpDefaultException;
        }
        ho_Image.Dispose();
        ho_SymbolXLDs.Dispose();

        hv_DataCodeHandle.Dispose();
        hv_ResultHandles.Dispose();
        hv_DecodedDataStrings.Dispose();
        hv_GradingResults.Dispose();
        hv_JsonString.Dispose();

    }

}
