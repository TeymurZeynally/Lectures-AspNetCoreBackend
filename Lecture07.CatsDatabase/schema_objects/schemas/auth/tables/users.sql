CREATE TABLE IF NOT EXISTS auth.users (
    id              BIGSERIAL       NOT NULL    PRIMARY KEY,
    uid             UUID            NOT NULL    UNIQUE DEFAULT gen_random_uuid(),
    username        VARCHAR(50)     NOT NULL    UNIQUE,
    email           VARCHAR(100)    NOT NULL    UNIQUE,
    password        VARCHAR(255)    NOT NULL,
    created_at      TIMESTAMPTZ     NOT NULL    DEFAULT CURRENT_TIMESTAMP
);