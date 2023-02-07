CREATE SCHEMA todoapi;

-- Table: todoapi.user

-- DROP TABLE IF EXISTS todoapi."user";

CREATE TABLE IF NOT EXISTS todoapi."user"
(
    id integer NOT NULL DEFAULT nextval('todoapi.todo_user_id_seq'::regclass),
    email text COLLATE pg_catalog."default" NOT NULL,
    password text COLLATE pg_catalog."default" NOT NULL,
    created timestamp without time zone NOT NULL,
    updated timestamp without time zone NOT NULL,
    CONSTRAINT user_pk PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS todoapi."user"
    OWNER to postgres;

-- Type: todo_status

-- DROP TYPE IF EXISTS public.todo_status;

CREATE TYPE public.todo_status AS ENUM
    ('NotStarted', 'OnGoing', 'Completed');

ALTER TYPE public.todo_status
    OWNER TO postgres;

-- Table: todoapi.todo

-- DROP TABLE IF EXISTS todoapi.todo;

CREATE TABLE IF NOT EXISTS todoapi.todo
(
    id integer NOT NULL DEFAULT nextval('todoapi.todo_id_seq'::regclass),
    name text COLLATE pg_catalog."default" NOT NULL,
    description text COLLATE pg_catalog."default",
    user_id integer NOT NULL DEFAULT nextval('todoapi.todo_user_id_seq'::regclass),
    created timestamp without time zone NOT NULL,
    updated timestamp without time zone NOT NULL,
    status todo_status NOT NULL,
    CONSTRAINT todo_pk PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS todoapi.todo
    OWNER to postgres;