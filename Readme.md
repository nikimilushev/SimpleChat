Small Chat System.

Features:
1. Chat conversations between several participants.

2. Audit the participant names and the start & end time in the DB.

3. Audit all the conversation enters in the DB (SQL Server).

4. Write WCF service with JSON serialization hosted in IIS (using DTO).

5. Use entity framework as a DAL layer.

Instructions:

1. Use CreateDatabase.sql to init the database.
2. Edit SimleChat.Server\Web.config to replace the name of the sql server "localhost\sqlexpress" with something that will work for you.
3. Run Visual Studio "as administrator" if you want to run the tests
4. The communication port is 12345. If you want to change it there are two places to be affected: 
  - SimpleChat.Server -> properties -> web
  - SumplaChat.UI -> App.config -> ServerAddress
5. Set SimpleChat.Server as startup project, press f5, then use right click on SimpleChat.UI, Debug, Start New Instance to instantiate several chat clients.

