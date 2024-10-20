using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using TodoListApp.Api.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("ModifyAccountPermission", policy =>
	{
		policy.RequireClaim("Permission", "ModifyAccount");
	});

	options.AddPolicy("CreateNewAccountNotePermission", policy =>
	{
		policy.RequireClaim("Permission", "CreateNewAccountNote");
	});

	options.AddPolicy("GetPermission", policy =>
	{
		policy.RequireClaim("Permission", "Get");
	});

	options.AddPolicy("PostPermission", policy =>
	{
		policy.RequireClaim("Permission", "Post");
	});

	options.AddPolicy("PutPermission", policy =>
	{
		policy.RequireClaim("Permission", "Put");
	});

	options.AddPolicy("DeletePermission", policy =>
	{
		policy.RequireClaim("Permission", "Delete");
	});
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = TokenProvider.ISSUER,
			ValidateAudience = true,
			ValidAudience = TokenProvider.AUDIENCE,
			ValidateLifetime = true,
			IssuerSigningKey = TokenProvider.GetSecurityKey(),
			ValidateIssuerSigningKey = true
		};
	});

builder.Services.AddControllers()
	.AddJsonOptions(
		options =>
		options.JsonSerializerOptions
		.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddCors();


builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Jwt-token:",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "bearer"
	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference=new OpenApiReference
				{
					Type=ReferenceType.SecurityScheme,
					Id="Bearer"
				}
			},
			new string[]{}
		}
	});
});



var app = builder.Build();

app.UseCors(builder =>
{
	builder.AllowAnyOrigin();
	builder.AllowAnyMethod();
	builder.AllowAnyHeader();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
		options.RoutePrefix = string.Empty;
	});
}
app.Run();