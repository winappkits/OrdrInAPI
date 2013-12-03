Ordr.In API
=================
#Date: 12.03.2013
#Version: v1.0.0
#Author(s): Steve Maier

###Features
 - Retrieves restaurant delivery locations based on a given address
 - Can retrieve restaurant location and display it using Bing Maps
 - Can place orders for food to be delivered

###Requirements
 - Windows 8
 - Visual Studio 2012 or higher
 - JSON.NET form Newtonsoft (https://json.codeplex.com/)
 - Ordr.In Developer Account/App (http://hackfood.ordr.in/)
 - BingMap Developer Account/App (http://www.microsoft.com/maps/create-a-bing-maps-key.aspx)
 
###Setup
 1. Create an developer account with Ordr.In at http://hackfood.ordr.in/ to obtain the developer key needed in the app starter kit.  Create a Bing Map developer account and get a developer key at http://www.microsoft.com/maps/create-a-bing-maps-key.aspx .  These keys are both needed to compile the sample project.
 2. Download the Starter Kit from (http://apimash.github.io/StarterKits/)
 3. Open the ***APIMASH_Ordr.In_StarterKit_Phone*** Solution in Visual Studio 2012
 4. Add your Developer Keys for Ordr.In and Bing Maps in the ***APIMASH_OrdrInLib*** project in the ***APIMASH_OrdrIn.cs*** file
 5. Compile and Run

###Warning
In the ***APIMASH_OrdrInLib*** project in the ***APIMASH_OrdrIn.cs*** file you will notice that there are variables setup to point you to the test or production servers: testingRestaurantAddress, testingOrderAddress, testingUserAddress.  This is setup by default to point to the test servers.  If you change to point to the production servers, this will actually charge your credit card and food will be delivered to your delivery location.  Microsoft and I are not responsible for the food or your credit card charges that are accrued using this starter kit.  

##DISCLAIMER: 

The sample code described herein is provided on an "as is" basis, without warranty of any kind, to the fullest extent permitted by law. Both Microsoft and I do not warrant or guarantee the individual success developers may have in implementing the sample code on their development platforms or in using their own Web server configurations. 

Microsoft and I do not warrant, guarantee or make any representations regarding the use, results of use, accuracy, timeliness or completeness of any data or information relating to the sample code. Microsoft and I disclaim all warranties, express or implied, and in particular, disclaims all warranties of merchantability, fitness for a particular purpose, and warranties related to the code, or any service or software related thereto. 

Microsoft and I shall not be liable for any direct, indirect or consequential damages or costs of any type arising out of any action taken by you or others related to the sample code.


----------

