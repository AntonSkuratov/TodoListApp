using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using TodoListApp.Api.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthorization(options =>
{
	//Добавление политик авторизации
	options.AddPolicy("ModifyAccountPermission", policy =>
	{
		policy.RequireClaim("Permission", "ModifyAccount");
	});

	options.AddPolicy("GetCurrentUserData", policy =>
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

//Подключение сервиса аутентификации
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
			ValidateIssuerSigningKey = true,
			ClockSkew = TimeSpan.Zero
		};
	});

//Подключение сервиса регистрации контроллеров
builder.Services.AddControllers()
	/*
	 *Для решения проблемы возвращения null-значения при подключении
	 *зависимых сущностей в методах Include/ThenInclude и отображения в Swagger
	 */
	.AddJsonOptions(
		options =>
		options.JsonSerializerOptions
		.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//Подключение сервиса корсов для отправки http запросов на сервер 
builder.Services.AddCors();

//Подключение сервиса SwaggerGen
builder.Services.AddSwaggerGen(options =>
{
	//Добавление поля для ввода токена в интерфейсе Swagger
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
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[]{}
		}
	});
});



var app = builder.Build();

//Подключение middleware UseCors для обработки запросв с любых доменов
app.UseCors(builder =>
{
	builder.AllowAnyOrigin();
	builder.AllowAnyMethod();
	builder.AllowAnyHeader();
});


//Подключение middleware для работы аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();


//Подкдючение middleware для добавления конечных точек для действий контроллеров
app.MapControllers();


//Если приложение находится в стадии разработки, то используется Swagger
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