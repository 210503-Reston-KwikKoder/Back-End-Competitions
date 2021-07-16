FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app 

COPY *.sln .

COPY CBERest/*.csproj ./CBERest/
COPY CBEBL/*.csproj ./CBEBL/
COPY CBEDL/*.csproj ./CBEDL/
COPY CBEModels/*.csproj ./CBEModels/
COPY CBETests/*.csproj ./CBETests/

RUN cd CBERest && dotnet restore

COPY . ./


RUN dotnet publish CBERest -c Release -o publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app 

COPY --from=build /app/publish ./
CMD ["dotnet", "CBERest.dll"]