CREATE TABLE "two_factor_phonedevice"(
    "id" INT NOT NULL,
    "name" NVARCHAR(255) NOT NULL,
    "confirmed" TINYINT NOT NULL,
    "number" NVARCHAR(255) NOT NULL,
    "key" NVARCHAR(255) NOT NULL,
    "method" NVARCHAR(255) NOT NULL,
    "user_id" INT NOT NULL
);
ALTER TABLE
    "two_factor_phonedevice" ADD CONSTRAINT "two_factor_phonedevice_id_primary" PRIMARY KEY("id");
CREATE INDEX "two_factor_phonedevice_user_id_index" ON
    "two_factor_phonedevice"("user_id");
CREATE TABLE "auth_user"(
    "id" INT NOT NULL,
    "password" NVARCHAR(255) NOT NULL,
    "last_login" DATETIME NULL,
    "is_superuser" TINYINT NOT NULL,
    "username" NVARCHAR(255) NOT NULL,
    "first_name" NVARCHAR(255) NOT NULL,
    "last_name" NVARCHAR(255) NOT NULL,
    "email" NVARCHAR(255) NOT NULL,
    "is_staff" TINYINT NOT NULL,
    "is_active" TINYINT NOT NULL,
    "date_joined" DATETIME NOT NULL
);
ALTER TABLE
    "auth_user" ADD CONSTRAINT "auth_user_id_primary" PRIMARY KEY("id");
CREATE UNIQUE INDEX "auth_user_username_unique" ON
    "auth_user"("username");
ALTER TABLE
    "two_factor_phonedevice" ADD CONSTRAINT "two_factor_phonedevice_user_id_foreign" FOREIGN KEY("user_id") REFERENCES "auth_user"("id");