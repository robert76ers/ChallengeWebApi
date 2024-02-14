FROM mcr.microsoft.com/dotnet/sdk AS BUILD
WORKDIR challengeWebApi

EXPOSE 80
EXPOSE 8086

COPY ./*.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o ChallengeWebApi

FROM mcr.microsoft.com/dotnet/sdk
WORKDIR /challengeWebApi
COPY --frombuild /challengeWebApi/ChallengeWebApi
ENTRYPOINT ["dotnet", "N5ChallengeWebApi.dll"]