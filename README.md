Frontend setup sequence:
1. run "npm i" first
2. to serve, run "npm run dev"
3. to test, run "npm test"


Backend setup sequence:
1. Change the "DbConnStr" found in appsettings.json,
modify it to a valid Postgres connection string. Make sure the database exists
OR set the credentials that has permission to create DB(I've used superuser for
this just so there would be no issues).
2. start via F5