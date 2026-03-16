CREATE TABLE IF NOT EXISTS cats.cats (
    id              BIGSERIAL       NOT NULL    PRIMARY KEY,
    user_id         BIGINT          NOT NULL,
    uid             UUID            NOT NULL    UNIQUE DEFAULT gen_random_uuid(),
    name            VARCHAR(50)     NOT NULL,
    breed           VARCHAR(50)     NOT NULL,
    age             INT             NOT NULL,
    created_at      TIMESTAMPTZ     NOT NULL    DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT chk_cats_age CHECK (age >= 0),
    CONSTRAINT fk_cats_user FOREIGN KEY (user_id) REFERENCES auth.users(id)
);