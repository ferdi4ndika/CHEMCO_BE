////using Entities.Exceptions;
//using Microsoft.AspNetCore.Mvc.Filters;

////using Newtonsoft.Json.Converters;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;
////using JsonSerializerNewtonsoft = Newtonsoft.Json.JsonSerializer;
////using JsonConverterNewtonsoft = Newtonsoft.Json;
//namespace MiniSkeletonAPI.Presentation.Helpers
//{
//    public class DateTimeFormatConverter : JsonConverter<DateTime>
//    {
//        private readonly string format;

//        public DateTimeFormatConverter(string format)
//        {
//            this.format = format;
//        }

//        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        {
//            try
//            {
//                return DateTime.ParseExact(
//                reader.GetString(),
//                format,
//                new CultureInfo("id-ID"));
//            }
//            catch (Exception ex)
//            {
//                throw new GeneralBadRquest(ex.Message);
//            }
//        }

//        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
//        {
//            ArgumentNullException.ThrowIfNull(writer, nameof(writer));
//            writer.WriteStringValue(value
//                //.ToUniversalTime()
//                .ToString(
//                    format,
//                    CultureInfo.InvariantCulture));
//        }
//    }
//    public class DateTimeConverterActionFilter : IActionFilter
//    {
//        public void OnActionExecuting(ActionExecutingContext context)
//        {
//            foreach (var argument in context.ActionArguments)
//            {
//                if (argument.Value is DateTime dateTime)
//                {
//                    // Tambahkan 7 jam ke dalam tanggal
//                    DateTime convertedDate = dateTime.AddHours(7);
//                    context.ActionArguments[argument.Key] = convertedDate;
//                }
//            }
//        }

//        public void OnActionExecuted(ActionExecutedContext context)
//        {
//            // Kosongkan, tidak perlu diimplementasi untuk saat ini
//        }
//    }
//    //public class NewtonSoftDateTimeFormatConverter : DateTimeConverterBase
//    //{
//    //    private const string Format = "yyyy-MM-ddTHH:mm:ss.fffZ";
//    //    public NewtonSoftDateTimeFormatConverter()
//    //    {
//    //        Console.WriteLine(Format);
//    //    }
//    //    public override void WriteJson(JsonConverterNewtonsoft.JsonWriter writer, object value, JsonSerializerNewtonsoft serializer)
//    //    {
//    //        Console.WriteLine("MASUK NEWTON");
//    //        if (value is DateTime dateTime)
//    //        {
//    //            writer.WriteValue(dateTime.ToString(Format));
//    //        }
//    //        else
//    //        {
//    //            throw new JsonConverterNewtonsoft.JsonSerializationException("Expected DateTime object value.");
//    //        }
//    //    }

//    //    public override object ReadJson(JsonConverterNewtonsoft.JsonReader reader, Type objectType, object existingValue, JsonSerializerNewtonsoft serializer)
//    //    {
//    //        Console.WriteLine("KELUAR NEWTON");

//    //        if (reader.TokenType == JsonConverterNewtonsoft.JsonToken.String)
//    //        {
//    //            //if (DateTime.TryParseExact(reader.Value.ToString(), Format, null, System.Globalization.DateTimeStyles.None, out DateTime result))
//    //            //{
//    //            //    return result;
//    //            //}
//    //            return DateTime.Parse(
//    //              reader.Value.ToString(),
//    //              new CultureInfo("id-ID"));
//    //        }
//    //        throw new JsonConverterNewtonsoft.JsonSerializationException("Unable to parse the DateTime value.");
//    //    }
//    //}


//    //public class NewtonSoftDateTimeFormatConverter : Newtonsoft.Json.JsonConverter<DateTime>
//    //{
//    //    private readonly string format;

//    //    public NewtonSoftDateTimeFormatConverter(string format)
//    //    {
//    //        this.format = format;
//    //    }

//    //    public override DateTime ReadJson(ref JsonReader reader, Type typeToConvert, Newtonsoft.Json.JsonSerializer options)
//    //    {
//    //        Console.WriteLine("KENAPA GA KELUAR");
//    //        try
//    //        {
//    //            return DateTime.ParseExact(
//    //            reader.ReadAsString(),
//    //            this.format,
//    //            CultureInfo.InvariantCulture);
//    //        }
//    //        catch (Exception ex)
//    //        {
//    //            throw new GeneralBadRquest(ex.Message);
//    //        }
//    //    }

//    //    public override void WriteJson(JsonWriter writer, DateTime value, Newtonsoft.Json.JsonSerializer options)
//    //    {
//    //        Console.WriteLine("KENAPA GA 2");
//    //        ArgumentNullException.ThrowIfNull(writer, nameof(writer));
//    //        writer.Write(value
//    //            //.ToUniversalTime()
//    //            .ToString(
//    //                this.format,
//    //                CultureInfo.InvariantCulture));
//    //    }
//    //}

//    //public sealed class JsonDateTimeFormatAttribute : JsonConverterAttribute
//    //{
//    //    private readonly string format;

//    //    public JsonDateTimeFormatAttribute(string format)
//    //    {
//    //        this.format = format;
//    //    }

//    //    public string Format => this.format;

//    //    public override JsonConverter? CreateConverter(Type typeToConvert)
//    //    {
//    //        return new DateTimeFormatConverter(this.format);
//    //    }
//    //}
//}
