using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BasicApi.Common.Mvc
{
    public class ErrorResponse
    {
        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; }

        [JsonPropertyName("errorDescription")]
        public string ErrorDescription { get; set; }

        [JsonPropertyName("trace")]
        public string Trace { get; set; }

        [JsonPropertyName("inner")]
        public string Inner { get; set; }

        [JsonPropertyName("errors")]
        public List<ModelStateErrorDto> Errors { get; set; }
    }

    public class ModelStateErrorDto
    {
        [JsonPropertyName("fieldName")]
        public string FieldName { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

    }
}
