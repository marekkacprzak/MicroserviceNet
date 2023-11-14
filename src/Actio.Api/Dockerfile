FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /dotnetapp
COPY ./bin/Docker .
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker
ENTRYPOINT dotnet Actio.Api.dll
#ENTRYPOINT ["/bin/sh", "-c" , "echo 127.0.0.1  rabbitmq >> /etc/hosts &&  dotnet Actio.Api.dll" ]
#CMD ["sh", "-c", "tail -f /dev/null"]
