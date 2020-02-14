# healthcare_ssi
Proof of concept for Self Sovereign Identity (SSI) involving Patients, Insurance Companies and Hospitals.

HealthCare SSI

CONTENTS OF THIS FILE
---------------------
   
 * Introduction
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
for the specific patient. This solution use assymetric keys
and the SHA-512 hashing algo to prove hospital signatures.

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
* Clone the repo
* Perform a Nuget restore of all packages
* setup or use appsettings.Local.json for local dev
* Set ASPNETCORE_ENVIRONMENT var with `$env:ASPNETCORE_ENVIRONMENT='Local'`
* Create db local with command `Update-Database`

 CONFIGURATION
------------

HealthSSI is a .Net core application which uses appsettings.json files
for Configuration. appsettings.json serves as the base config file. It 
supports additional config files for environment specific variables.
Locally or on the server, ASPNETCORE_ENVIRONMENT needs to be set 
which will instruct .Net core to read in the appropriate settings file.

 INFRASTRUCTURE
------------

HealthSSI is currently deployed in AWS using EBS and MS SQL on RDS. 
http://healthssi-env.uxpb73mr6m.us-east-1.elasticbeanstalk.com/
DB: healthssi-db.cvzjpppmueiv.us-east-1.rds.amazonaws.com
The database schema is currently being maintained via code first migrations.
Deployments currently being done via Visual Studio AWS Toolkit.
https://aws.amazon.com/visualstudio/

 TESTS
------------

There are two test projects: 
* HealthSSI.Core.Tests (unit tests)
* HealthCareSSIApi.IntegrationTests (integration tests)
* HealthCareSSI.Tests.Common (shared test lib)

 NEXT STEPS
------------

Add authentication to API
Containerize project
Setup CI/CD
Add integration with Sovrin network
Separate out into separate services if need be?
...
