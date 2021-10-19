CREATE DATABASE pantry
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'English_United States.1252'
    LC_CTYPE = 'English_United States.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

\c pantry

CREATE TABLE IF NOT EXISTS public."Account"
(
    "acc_ID" uuid NOT NULL,
    firstname character varying(50) COLLATE pg_catalog."default" NOT NULL,
    email character varying(127) COLLATE pg_catalog."default" NOT NULL,
    password character varying(64) COLLATE pg_catalog."default" NOT NULL,
    lastname character varying(50) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Account_pkey" PRIMARY KEY ("acc_ID"),
    CONSTRAINT "Account_email_key" UNIQUE (email)
)

TABLESPACE pg_default;

ALTER TABLE public."Account"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public."Ingredients"
(
    "ingredient_ID" integer NOT NULL,
    name text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Ingredients_pkey" PRIMARY KEY ("ingredient_ID")
)

TABLESPACE pg_default;

ALTER TABLE public."Ingredients"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public."Products"
(
    "item_ID" text COLLATE pg_catalog."default" NOT NULL,
    quantity text COLLATE pg_catalog."default" NOT NULL,
    category text COLLATE pg_catalog."default" NOT NULL,
    name text COLLATE pg_catalog."default" NOT NULL,
    price money,
    searchtag text COLLATE pg_catalog."default" NOT NULL,
    "ingredient_ID" integer NOT NULL,
    CONSTRAINT "Products_pkey" PRIMARY KEY ("item_ID"),
    CONSTRAINT "Products_ingredient_ID_fkey" FOREIGN KEY ("ingredient_ID")
        REFERENCES public."Ingredients" ("ingredient_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
)

TABLESPACE pg_default;

ALTER TABLE public."Products"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public."Recipes"
(
    "recipe_ID" integer NOT NULL,
    "recipe_Description" text COLLATE pg_catalog."default" NOT NULL,
    recipe_name text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Recipes_pkey" PRIMARY KEY ("recipe_ID")
)

TABLESPACE pg_default;

ALTER TABLE public."Recipes"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public."Inventory_List"
(
    "item_ID" text COLLATE pg_catalog."default" NOT NULL,
    "acc_ID" uuid NOT NULL,
    exp_date date NOT NULL,
    count integer,
    notification_time date,
    CONSTRAINT "Inventory_List_pkey" PRIMARY KEY ("item_ID", "acc_ID", exp_date),
    CONSTRAINT "Inventory_List_acc_ID_fkey" FOREIGN KEY ("acc_ID")
        REFERENCES public."Account" ("acc_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT "Inventory_List_item_ID_fkey" FOREIGN KEY ("item_ID")
        REFERENCES public."Products" ("item_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."Inventory_List"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public."Recipe_List"
(
    "recipe_ID" integer NOT NULL,
    "acc_ID" uuid NOT NULL,
    CONSTRAINT "Recipe_List_pkey" PRIMARY KEY ("recipe_ID", "acc_ID"),
    CONSTRAINT "Recipe_List_acc_ID_fkey" FOREIGN KEY ("acc_ID")
        REFERENCES public."Account" ("acc_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT "Recipe_List_recipe_ID_fkey" FOREIGN KEY ("recipe_ID")
        REFERENCES public."Recipes" ("recipe_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."Recipe_List"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public."Shopping_List"
(
    "item_ID" text COLLATE pg_catalog."default" NOT NULL,
    "acc_ID" uuid NOT NULL,
    count integer NOT NULL,
    CONSTRAINT "Shopping_List_pkey" PRIMARY KEY ("item_ID", "acc_ID"),
    CONSTRAINT "Shopping_List_acc_ID_fkey" FOREIGN KEY ("acc_ID")
        REFERENCES public."Account" ("acc_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT "Shopping_List_item_ID_fkey" FOREIGN KEY ("item_ID")
        REFERENCES public."Products" ("item_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."Shopping_List"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public."Recipe_Steps"
(
    "recipe_ID" integer NOT NULL,
    "step_ID" integer NOT NULL,
    instructions text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Recipe_Steps_pkey" PRIMARY KEY ("recipe_ID", "step_ID"),
    CONSTRAINT "Recipe_Steps_recipe_ID_fkey" FOREIGN KEY ("recipe_ID")
        REFERENCES public."Recipes" ("recipe_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."Recipe_Steps"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public."Recipe_Ingredients"
(
    "recipe_ID" integer NOT NULL,
    "ingredient_ID" integer NOT NULL,
    amount real NOT NULL,
    unit_of_measure text COLLATE pg_catalog."default" NOT NULL,
    name text COLLATE pg_catalog."default" NOT NULL,
    "originalName" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Recipe_Ingredients_pkey" PRIMARY KEY ("recipe_ID", "ingredient_ID", "originalName"),
    CONSTRAINT "Recipe_Ingredients_recipe_ID_fkey" FOREIGN KEY ("recipe_ID")
        REFERENCES public."Recipes" ("recipe_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."Recipe_Ingredients"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public."Subscription"
(
    "acc_ID" uuid NOT NULL,
    sub_endpoint text COLLATE pg_catalog."default" NOT NULL,
    key text COLLATE pg_catalog."default" NOT NULL,
    audh text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Subscription_pkey" PRIMARY KEY ("acc_ID"),
    CONSTRAINT "Subscription_acc_ID_fkey" FOREIGN KEY ("acc_ID")
        REFERENCES public."Account" ("acc_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."Subscription"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public."Admin"
(
    acc_id uuid NOT NULL,
    level integer NOT NULL,
    CONSTRAINT "Admin_pkey" PRIMARY KEY (acc_id),
    CONSTRAINT "Admin_acc_id_fkey" FOREIGN KEY (acc_id)
        REFERENCES public."Account" ("acc_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."Admin"
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public."Recipe_document"
(
    "recipe_ID" integer NOT NULL,
    url text COLLATE pg_catalog."default" NOT NULL,
    "photoUrl" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Recipe_document_pkey" PRIMARY KEY ("recipe_ID"),
    CONSTRAINT "Recipe_document_recipe_ID_fkey" FOREIGN KEY ("recipe_ID")
        REFERENCES public."Recipes" ("recipe_ID") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public."Recipe_document"
    OWNER to postgres;

