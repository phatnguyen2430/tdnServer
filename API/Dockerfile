FROM microsoft/dotnet:latest
COPY . /app
WORKDIR /app

RUN dotnet restore


RUN dotnet tool install --global dotnet-ef
RUN dotnet add package Microsoft.EntityFrameworkCore.Design
RUN dotnet ef migrations add InitialCreate
RUN dotnet ef database update

RUN dotnet build


EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker


ENTRYPOINT dotnet run