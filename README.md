# Sistema de Gerenciamento de Reservas

## Itens necessários:

- .NET SDK
- [RabbitMQ](https://www.rabbitmq.com/docs/download) (Baixando localmente ou utilizando o Docker)

## 1. Rodar o comando SQL

Use o seguinte script SQL para criacão de tabelas:

```sql
CREATE DATABASE gerenciamento_reservas;
USE gerenciamento_reservas;

CREATE TABLE users (
	id UUID PRIMARY KEY,
	email VARCHAR(100) UNIQUE NOT NULL,
	password_hash VARCHAR(100) NOT NULL
);

CREATE TABLE hotels (
	id SERIAL PRIMARY KEY,
	name VARCHAR(100) NOT NULL,
	country VARCHAR(100),
	city VARCHAR(100),
	address VARCHAR(150),
	price_per_night NUMERIC NOT NULL CHECK (price_per_night > 0)
);

CREATE TABLE bookings (
	id SERIAL PRIMARY KEY,
	user_id UUID NOT NULL,
	hotel_id INTEGER NOT NULL,
	room_number INTEGER,
	start_date TIMESTAMPTZ,
	end_date TIMESTAMPTZ,
	status VARCHAR(50),
	FOREIGN KEY (user_id) REFERENCES users(id),
	FOREIGN KEY (hotel_id) REFERENCES hotels(id) ON DELETE CASCADE
);
```

## 2. Clone o repositório

No `cmd`, executar o comando:

```bash
git clone https://github.com/mateusty/DesafioTecnicoBackend_GerenciamentoReservas.git
```

Então mude para o diretório do projeto

```bash
cd ./DesafioTecnicoBackend_GerenciamentoReservas
```

Crie um arquivo .env no caminho `/src/Presentation` com o seguinte conteúdo:

```env
ConnectionStrings__DefaultConnection=

JwtSettings__SecretKey=

```

Preenchendo os dados de forma adequada


```bash
dotnet run --project "./src/Presentation"
```

A API estará disponível na URL
```
http://localhost:5234
```
