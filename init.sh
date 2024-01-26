#!/bin/bash
while :
do 
  PORTS=`lsof -i:1433`
  echo $PORTS
  if ["$PORTS" = '']; then
    echo 'hotel-database-1 is down, trying to reconnect to run initial migrations!'
    sleep 5s
  else
    docker exec hotel-database-1 /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TrybeHotel12! -d master -i initdb/initial.sql; echo "All done!";
    break
  fi
done

echo "successfuly executed!"