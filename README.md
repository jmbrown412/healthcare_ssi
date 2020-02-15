# healthcare_ssi
 Present a way to verify that medical documents were in issued by the hospital and that it is issued for the family member making the claim. 

HealthCare SSI

CONTENTS OF THIS FILE
---------------------
   
 * Introduction
 * Project Structure
 * Requirements
 * Installation
 * Configuration
 * Infrastructure
 * Tests
 * Next steps

 INTRODUCTION
------------

This project is a proof of concept for the health care industry
demonstrating the ability for hospitals, insurance companies 
and pateients to act in a decentralized manner where hospitals
can issue documents for patients and the insurance companies 
can trust that the documents were indeed signed by the hospital
for the specific patient. This solution use digital signatures.

 PROJECT STRUCTURE
------------

* HealthCareSSIApi - Web API around core business logic
* HealthSSI.Core - Core business logic using digital signatures
* HealthSSI.Data - Data layer for Database (Currently MSSQL)
* HealthCareSSI.Tests.Common - Common test library for helper functions for things like getting a key pair
* HealthSSI.Core.Tests - Unit tests for signature and document proof checking
* HealthSSIApi.IntegrationTests - Integration tests testing end to end case of hospital document inssuance verification

 REQUIREMENTS
------------

HealthSSI requires the following:
* .Net core (3.0.0)
* Newtonsoft.Json (12.0.3)
* RestSharp (106.10.1)
* Swashbucle.AspNetCore (Swagger) (5.0.0)

 INSTALLATION
------------

To install locally, do the following:
1. Clone the repo
2. Perform a Nuget restore of all packages
3. setup or use appsettings.Local.json for local dev
4. Set ASPNETCORE_ENVIRONMENT var with `$env:ASPNETCORE_ENVIRONMENT='Local'`
5. Create db local with command `Update-Database`

 CONFIGURATION
------------

HealthSSI is a .Net core application which uses appsettings.json files
for Configuration. appsettings.json serves as the base config file. It 
supports additional config files for environment specific variables.
Locally or on the server, ASPNETCORE_ENVIRONMENT needs to be set 
which will instruct .Net core to read in the appropriate settings file.

 INFRASTRUCTURE
------------

* HealthSSI is currently deployed in AWS using EBS and MS SQL on RDS. http://healthssi-env.uxpb73mr6m.us-east-1.elasticbeanstalk.com/
* DB: healthssi-db.cvzjpppmueiv.us-east-1.rds.amazonaws.com
* The database schema is currently being maintained via code first migrations.
* Deployments currently being done via Visual Studio AWS Toolkit.
* https://aws.amazon.com/visualstudio/

 TESTS
------------

There are three test projects: 
* HealthSSI.Core.Tests (unit tests)
* HealthCareSSIApi.IntegrationTests (integration tests)
* HealthCareSSI.Tests.Common (shared test lib)

 NEXT STEPS
------------

* Use HSM for keys - AWS supports HSM which is FIPS 140-2 level 3 
* https://aws.amazon.com/cloudhsm/ 
* https://en.wikipedia.org/wiki/FIPS_140-2
* Add authentication to API
* Containerize project
* Setup CI/CD
* Add integration with Sovrin network
* Separate out into separate services if need be?
* Hash contents of Document and place on a blockchain
