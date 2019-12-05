sudo docker run --rm -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=<YourStrong@Passw0rd>" \
   -p 1433:1433 --name AutoReservationSqlServer \
   -d mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04
