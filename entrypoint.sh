#!/bin/bash
# Aguarda o SQL Server iniciar antes de rodar o script
echo "Aguardando SQL Server iniciar..."
sleep 15s

# Executa o script SQL de inicialização
echo "Executando script de inicialização..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -C -U sa -P "YourStrong!Passw0rd" -d master -i /docker-entrypoint-initdb.d/init.sql

echo "Banco de dados inicializado com sucesso!"

# Mantém o container rodando
exec /opt/mssql/bin/sqlservr
