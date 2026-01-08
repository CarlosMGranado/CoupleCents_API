# Pull MySQL image
docker pull mysql:8.0

# Run MySQL container
docker run --name mysql-dev \
  -e MYSQL_ROOT_PASSWORD=Aa123456 \
  -e MYSQL_DATABASE=CoupleCentsDB \
  -e MYSQL_USER=dev_user \
  -e MYSQL_PASSWORD=Dev123! \
  -p 3306:3306 \
  -d mysql: 8.0

# Verify it's running
docker ps