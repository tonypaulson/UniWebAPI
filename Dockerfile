FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app 
COPY . ./
RUN dotnet publish ./CWL.VirtualCare.API/CWL.VirtualCare.API.csproj -c Debug -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
RUN apt update
RUN apt install unzip procps -y
RUN curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l /vsdbg
EXPOSE 80
WORKDIR /app
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.2.1/wait /wait
RUN chmod +x /wait
COPY --from=build-env /app/out .
CMD /wait && dotnet CWL.VirtualCare.API.dll
