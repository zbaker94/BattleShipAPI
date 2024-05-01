#!/bin/bash
PASSWORD="m=3q>0Th%fJ7k;9%7ntf7@AQT"
echo "password: $PASSWORD"
echo "Starting docker..."
if [ "$( docker container inspect -f '{{.State.Running}}' battleship_db )" == "true" ]; then
    echo "Container is already running. Stopping..."
    # stop container if it is running
    docker stop battleship_db
    echo "Container stopped."
fi

if [ "$( docker container inspect -f '{{.State.Running}}' battleship_db )" == "false" ]; then
    echo "Container exists. Removing..."
    # remove container if it exists
    docker rm battleship_db
    echo "Container removed."
fi

# pull latest image
docker pull mcr.microsoft.com/mssql/server:2019-latest

# run sql server image 
docker run --name battleship_db -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD='$PASSWORD  -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest

echo "Container started. Waiting for 30 seconds to create the database..."
# wait for the container to start
COUNT=0
while [ $COUNT -lt 30 ]; do
    sleep 1
    COUNT=$((COUNT+1))
    echo -n "."
done

echo "Done./n"

echo "Creating database..."
# run the sql script to create the database
sudo docker exec -it battleship_db /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $PASSWORD  -Q 'CREATE DATABASE BattleshipDB'
if [ $? -eq 0 ]; then
    echo "Database created."
fi
