FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .

# copy everything else and build app
RUN dotnet publish "BookyBook.Presentation" -c Release -o /BookyBook

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

WORKDIR /BookyBook
COPY --from=build /src .

EXPOSE 7790
VOLUME /BookyBook/SharedForlder

ENV MACHINE_NAME ${COMPUTERNAME}
ENTRYPOINT ["dotnet", "BookyBook.Presentation/bin/Release/net6.0/BookyBook.Presentation.dll"]
