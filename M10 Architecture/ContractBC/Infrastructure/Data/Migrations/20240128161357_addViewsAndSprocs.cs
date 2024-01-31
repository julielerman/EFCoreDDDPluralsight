using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class addViewsAndSprocs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE  View CurrentContractversions
            AS
            SELECT     ContractVersion_Authors.Name_LastName as LastName, ContractVersions.WorkingTitle, ContractVersions.ContractId, ContractVersion_Authors.Name_FirstName AS FirstName, Contracts.DateInitiated, Contracts.ContractNumber, Contracts.CurrentVersionId,
                       ContractVersions.Id AS versionid
            FROM       ContractVersion_Authors INNER JOIN
                       ContractVersions ON ContractVersion_Authors.ContractVersionId = ContractVersions.Id INNER JOIN
                       Contracts ON ContractVersions.ContractId = Contracts.Id AND ContractVersions.Id = Contracts.CurrentVersionId"
            );
            migrationBuilder.Sql(
             @"CREATE PROCEDURE GetContractsForAuthorLastNameStartswith
               @LastName varchar(15)
               AS
               select groupednames.contractId as KeyValue,[description]
               FROM
               (select contractid,currentversionid, dbo.BuildContractHighlights( dateinitiated,workingtitle,string_agg(FirstName + ' ' + LastName,',')) as [description] 
               from CurrentContractversions
               where currentversionid in 
                   (select currentversionid
                    from currentcontractversions
                    where left(LastName,len(trim(@LastName)))=trim(@LastName))
               group by CurrentVersionId,WorkingTitle,contractid,DateInitiated)  groupednames
            ");

            migrationBuilder.Sql(
             @"IF OBJECT_ID (N'BuildContractHighlights') IS NOT NULL
               DROP FUNCTION BuildContractHighlights
               GO
               
               CREATE FUNCTION BuildContractHighlights
               (	
               	@dateinitiated datetime, 
               	@workingtitle char(100),
               	@authorlist char(100))
               RETURNS nvarchar(250)
               AS
               -- place the body of the function here
               BEGIN
                      RETURN( 'Contract start:'+   FORMAT( @dateinitiated, 'd', 'en-US' ) +',""'+rtrim(@workingtitle) + '"", Author(s): '+ @authorlist)
               END
               GO
              ");
             migrationBuilder.Sql(
                @"CREATE PROCEDURE GetContractsInitiatedInDateRange
                  @initdatestart date,
                  @initdateend date
                  AS
                    select groupednames.contractId as KeyValue,[description]
                  FROM
                      (select contractid,currentversionid, dbo.BuildContractHighlights(dateinitiated,workingtitle,string_agg(FirstName + ' ' + LastName,',')) as description
                      from CurrentContractversions
                  WHERE currentversionid in 
                      (select currentversionid
                       from CurrentContractversions
                       where cast(dateinitiated as date)>=@initdatestart) and cast(dateinitiated as date)<=@initdateend  
                  GROUP BY [CurrentVersionId],WorkingTitle,contractid,DateInitiated ) groupednames
                 ");
        }

          protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP  View CurrentContractversions");
            migrationBuilder.Sql("DROP PROCEDURE GetContractsForAuthorLastNameStartswith");
            migrationBuilder.Sql("DROP FUNCTION BuildContractHighlights");
            migrationBuilder.Sql("DROP PROCEDURE GetContractsInitiatedInDateRange");

        }
    }
}
