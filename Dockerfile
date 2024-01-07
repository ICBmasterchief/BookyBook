FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY . .
RUN dotnet publish "BookyBook.Presentation/BookyBook.Presentation.csproj" -c Release -o /BookyBook

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /BookyBook
COPY --from=build /BookyBook .
EXPOSE 7790
VOLUME /BookyBook/SharedForlder
ENV MACHINE_NAME ${COMPUTERNAME}
ENTRYPOINT ["dotnet", "BookyBook.Presentation/BookyBook.Presentation.dll"]
