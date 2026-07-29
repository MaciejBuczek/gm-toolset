namespace Identity.API.OpenApi
{
    public class DefaultRequestProvider
    {
        public static OpenApiRequestBody RegisterRequest()
        {
            return new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    [MediaTypeNames.Application.Json] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.Schema,
                                Id = nameof(RegisterRequest)
                            }
                        },
                        Example = new OpenApiString(JsonSerializer.Serialize(new RegisterEndpoint.RegisterRequest(
                            Username: "GlimboTheGameMaster",
                            Email: "glimbo@example.com",
                            Password: "Pass1!"
                        )))
                    }
                }
            };

        }

        public static OpenApiRequestBody LoginRequest()
        {
            return new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    [MediaTypeNames.Application.Json] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.Schema,
                                Id = nameof(LoginEndpoint.LoginRequest)
                            }
                        },
                        Example = new OpenApiString(JsonSerializer.Serialize(new LoginEndpoint.LoginRequest(
                            Username: "GlimboTheGameMaster",
                            Email: "glimbo@example.com",
                            Password: "Pass1!"
                        )))
                    }
                }
            };
        }
    }
}
