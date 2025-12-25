# 1. AŞAMA: İNŞAAT (Build)
# Microsoft'un .NET 8 kutusunu kullanıyoruz
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Tüm dosyaları kopyala
COPY . .

# Projeyi onar ve paketle (Yayınlamaya hazırla)
# DİKKAT: Buradaki "SporSalonuTakip" kısımları senin proje klasörünün adıdır.
RUN dotnet restore "SporSalonuTakip/SporSalonuTakip.csproj"
RUN dotnet publish "SporSalonuTakip/SporSalonuTakip.csproj" -c Release -o /app/publish

# 2. AŞAMA: ÇALIŞTIRMA (Run)
# Sadece çalıştırma dosyalarını al (Daha hafif olur)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Sitenin çalışması için gereken port ayarı (Render otomatik atar ama biz de açalım)
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Başlatma komutu
ENTRYPOINT ["dotnet", "SporSalonuTakip.dll"]