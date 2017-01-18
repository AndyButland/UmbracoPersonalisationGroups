# Umbraco Personalisation Groups

## What it does

Umbraco Personalisation Groups is an Umbraco package intended to allow personalisation of content to different groups of site visitors.  

It can be downloaded and installed from: https://our.umbraco.org/projects/website-utilities/personalisation-groups

It contains a few different pieces:

- An interface for and various implementations of different personalisation group criteria (e.g. "time of day", "day of week")
- Implementation of the following criteria:
	- Authentication status
    - Cookie key presence/absence and value matching
	- Country (via IP matching)
    - Day of week
	- Number of site visits
	- Pages viewed
	- Querystring
	- Referrer
    - Session key presence/absence and value matching
    - Time of day
	- Umbraco member group
	- Umbraco member profile field	
	- Umbraco member type
- An extensible mechanism to allow other criteria to be created and loaded from other assemblies
- A property editor with associated angular controllers/views that provide the means of configuring personalisation groups based on the available criteria
- A extension methods on IPublishedContent and UmbracoHelper named ShowToVisitor() and ScoreForVisitor() that allows for showing, hiding or ordering ranking content for the current site visitor

## Using the package

### Installation

Firstly install the package in the usual way.  

Once installed you'll find a few additional components:

 - **Personalisation Groups Folder** is a document type purely used to organise your personalisation groups.  By default you can create it at root and within it create further instances of itself.
 - **Personalisation Group** is a document type where the group itself is created.  You can place these inside the folders.  It comes with a single property called *Group definition* of a data type also provided with this package.
 - **Personalisation group definition** the data type provided to allow the definition of personalisation group, based on a new property editor provided in the dll installed with the package.
 - **Personalisation group picker** is an data type instance of the multi-node tree picker property editor, for the selection of groups for given piece of content
 
There's also a NuGet installer if you prefer to use that:

    PM> Install-Package UmbracoPersonalisationGroups
	
However this will only install the dll, not the document types and data types.  As such I'd reccommend if you do want to use NuGet for ease of updates, do the following:

- Install the package from our.umbraco.org
- Then install from NuGet to get the dll as a package reference	 
 
### Example usage

 - Within the "Content" section, create a root node of type **Personalisation Groups Folder**, called **Personalisation Groups**
 - Switch to "Developer" and find the **Personalisation group picker** data type.  Set the root node to be the node you just created.
 - Back to "Content", as a child of the node you just created, create a node of type **Personalisation Group** called, for example, *Weekday morning visitors*
     - Set the **Match** option to **All**
	 - Set the **Duration in group** option to **Page**
	     - If you select other options here, the groups will become "sticky".  For example if someone comes to your home page that's personalised based on a querystring parameter, if they then return to the page by default they will no longer match the group (as the querystring value is no longer there).  But selecting **Session** or **Visitor** you can make the visitor stick to the group they matched originally (using a cookie).
	 - Set the **Score** option to **50**
	 - Add a new criteria of type **Day of week** and tick the boxes for Monday to Friday.
	 - Add a second criteria of type **Time of day** and add a range of 0000 to 1200
	 - Save and publish
	 
![Editing a group definition](/documentation/group-editing.png?raw=true "Editing a group definition")

![Editing a specific criteria](/documentation/definition-editing.png?raw=true "Editing a specific criteria")	 
	 
 - Now go to "Settings" and find the document type for a piece of content you want to personalise.  For example with the Fanoe Starter Kit you could select the *Blog Post* document type
 - Add a new field of type **Personsalisation group picker** with an alias of **personalisationGroups**.
    - If you don't like this alias you can use a different one, but you will also need to add the following appSetting key to your config file:
	
	```
	<add key="personalisationGroups.groupPickerAlias" value="myCustomAlias"/> 
	```	
	
 - Back to "Content" again, find or create a page of this document type and pick the **Weekday morning visitors** personalisation group
 
 ![Picking groups](/documentation/picking-groups.png?raw=true "Picking groups")	
 
 - Finally you need to amend your template to make use of the personalisation group via extension methods that will be available on instances of **IPublishedContent**, named **ShowToVisitor()** and/or **ScoreForVisitor()**, as described below.
 
## Templating
 
### Personalising repeated content - showing and hiding items in a list
 
A typical example would be to personalise a list of repeated content to only show items that are appropriate for the current site visitor.  Here's how you might do that:
 
 	@foreach (var post in Model.Content.Children.Where(x => x.ShowToVisitor()))
	{
	    <h2>@post.Name</h2>
    }
		
### Personalising page content
		
With a little more work you can also personalise an individual page.  One way to do this would be to create sub-nodes of a page of a new type called e.g. "Page Variation".  This document type should contain all the fields common to the parent page that you might want to personalise - e.g. title, body text, image - and an instance of the "Personalisation group picker".  You could then implement some logic on the parent page template to pull back the first of the sub-nodes that match the current site visitor.  If one is found, you can display the content from that sub-node rather than what's defined for the page.  And if not, display the default content for the page.  Something like:

	@{
		var personalisedContent = Model.Content.Children.Where(x => x.ShowToVisitor()).FirstOrDefault();
		string title, bodyText;
		if (personalisedContent != null) 
		{
			title = personalisedContent.Name;
			bodyText = personalisedContent.GetPropertyValue<string>("bodyText");
		}
		else 
		{
			title = Model.Content.Name;
			bodyText = Model.Content.GetPropertyValue<string>("bodyText");	
		}
	}
	
	<h1>@title</h1>
	<p>@bodyText</p>
	
Instead of using sub-nodes for the personalised information, this could just as well be items of [nested content](https://our.umbraco.org/projects/backoffice-extensions/nested-content/), given they also implement IPublishedContent.

### Personalising repeated content - ranking of items in a list

In addition to simply showing and hiding content, it's possible to rank a list of items to display them in order of relevence to the site visitor.  This can be achieved using the **Score** field for each created personsalisation group that can be set to a value between 1 and 100. These can either be set to all the same value, or more important groups can be given a higher score.

The following code will then determine which groups are associated with each item of content in the list, sum up the scores of those that match the site visitor and order with the highest score first:

    @{
        var personalisedContent = Model.Content.Children.OrderByDescending(x => x.ScoreForVisitor());
    }

	
## Configuration

No configuration is required if you are happy to accept the default behaviour of the package.  The following optional keys can be added to your web.config appSettings though if required to amend this.  

- `<add key="personalisationGroups.groupPickerAlias" value="myCustomAlias"/>` - amends the alias that must be used when creating a property field of type personalisation group picker
- `<add key="personalisationGroups.geoLocationCountryDatabasePath" value="/my/custom/relative/path"/>` - amends the convention path for where the IP-country geolocation database can be found see below for more details.
- `<add key="personalisationGroups.includeCriteria" value="alias1,alias2"/>` - provides the specific list of criteria to make available for creating personsaliation groups
- `<add key="personalisationGroups.excludeCriteria" value="alias1,alias2"/>` - provides a list of criteria to exclude from the full list of available criteria made available for creating personsaliation groups
- `<add key="personalisationGroups.numberOfVisitsTrackingCookieExpiryInDays" value="90"/>` - sets the expiry time for the cookie used for number of visits page tracking for the pages viewed criteria (default if not provided is 90)
- `<add key="personalisationGroups.viewedPagesTrackingCookieExpiryInDays" value="90"/>` - sets the expiry time for the cookie used for viewed page tracking for the pages viewed criteria (default if not provided is 90)
- `<add key="personalisationGroups.cookieKeyForTrackingNumberOfVisits" value="myCookieKey"/>` - defines the cookie key name used for tracking the number of visits
- `<add key="personalisationGroups.cookieKeyForTrackingIfSessionAlreadyTracked" value="myCookieKey"/>` - defines the cookie key name used for tracking if the session has been recorded for the number of visits criteria, default is `personalisationGroupsNumberOfVisitsSessionStarted`
- `<add key="personalisationGroups.cookieKeyForTrackingPagesViewed" value="myCookieKey"/>` - defines the cookie key name used for tracking pages viewed
- `<add key="personalisationGroups.cookieKeyForSessionMatchedGroups" value="myCookieKey"/>` - defines the cookie key name used for tracking which session level groups the visitor has matched
- `<add key="personalisationGroups.cookieKeyForPersistentMatchedGroups" value="myCookieKey"/>` - defines the cookie key name used for tracking which persistent (visitor) level groups the visitor has matched
- `<add key="personalisationGroups.persistentMatchedGroupsCookieExpiryInDays" value="90"/>` - sets the expiry time for the cookie used for tracking which persistent (visitor) level groups the visitor has matched (default if not provided is 90)


## How it works

### Personalisation group criteria (IPersonalisationGroupCriteria)

Group criteria all implement an interface **IPersonalisationGroupCriteria** which provides a few properties to identify and describe the criteria as well as a single method - **MatchesVisitor()**.

Implementations of this interface must provide logic in this method for checking whether the current site visitor matches the definition provided using a JSON syntax supported by the criteria.  So for example the **DayOfWeekPersonalisationGroupCriteria** expects a simple JSON array of day numbers - e.g. [1, 3, 5] - which is compared with the current day to determine a match.

### PersonalisationGroupMatcher

**PersonalisationGroupMatcher** is a static class that when first instantiated will scan all loaded assemblies for implementations of the IPersonalisationGroupCriteria interface and store references to them.  It's in this way the package will support extensions through the development of other criteria that may not be in the core package itself.

It then makes these criteria available to application logic that needs to create group definitions based on them and to check if a given definition matches the related criteria.

### PersonalisationGroupDefinitionPropertyEditor

**PersonalisationGroupDefinitionPropertyEditor** defines an Umbraco property editor for the definition of the personalisation groups.  It has a related angular view and controller, and also ensures the angular assets required for the specific criteria that are provided with the core package are loaded and available for use.

### PersonalisationGroupDefinitionController

**PersonalisationGroupDefinitionController** is a server-side controller that provides logic and resources to the angular view and controller used for the property editor.  It provides JSON end-points for the retrieval of the available criteria via HTTP requests.  It also provides methods for the retrieval of the angular assets that are provided as embedded resources in the package dll (or any extension dlls). See http://www.nibble.be/?p=415 for more detail on how this technique is implemented.

### Angular views and controllers

The primary view and controller for the property editor are **editor.html** and **editor.controller.js** respectively.

In addition to these, each criteria has it's own view and controller that provide a user friendly means of configuring the definitions, named **definition.editor.html** and **definition.editor.controller.js** which are loaded via a call to the Umbraco dialogService.  All are provided as embedded resources.

Each criteria also has an angular service named **definition.translator.js** responsible for translating the JSON syntax into something more human readable.  So again for example the **DayOfWeekPersonalisationGroupCriteria** will render "Sunday, Tuesday, Thursday" from [1, 3, 5].

### PublishedContentExtensions

**PublishedContentExtensions** defines the extension method on **IPublishedContent** named **ShowToVisitor(bool showIfNoGroupsDefined = true)**.  This implements the following logic:

- Checks for a group picker on the content.
    - If there's not one then we return the default value passed in the `showIfNoGroupsDefined` parameter (which if not provided, defaults to true, indicating to show to everyone).
- If found get the list of groups picked
    - If no groups are found then again we return the default value passed in the `showIfNoGroupsDefined` parameter.
- For each group picked, see if the definition provided matches the current site visitor.
    - If any one of them does, we return true (indicating to show the content)
	- If none of them do, we return false (indicating to hide the content)
	
There's also a related extension method on **UmbracoHelper** named **ShowToVisitor(IEnumerable<int> groupIds, bool showIfNoGroupsDefined = true)**.  Using this you can pass through a list of group Ids that may be drawn from another location than the current node.
	
## Notes on particular criteria

### Country

The country criteria uses the [free GeoLite2 IP to country database](http://dev.maxmind.com/geoip/geoip2/geolite2/) made available by Maxmind.com.  It'll look for it in /App_Data/GeoLite2-Country.mmdb or at the path specified in the following appSetting:

    <add key="personalisationGroups.geoLocationCountryDatabasePath" value="/my/custom/relative/path"/> 

### Pages viewed

In order to support personalising content to site visitors that have seen or not seen particular pages we need to track which pages they have viewed.  This is implemented using a cookie named **personalisationGroupsPagesViewed** that will be written and amended on each page request.  It has a default expiry of 90 days but you can amend this in configuration.  The cookie expiry slides, so if the site is used again before it expires, the values stored remain.	

If you don't want this cookie to be written, you can remove this criteria from the list available to select via configuration (see above).  If you do that, the criteria can't be used and the page tracking behaviour will be switched off.

## How to extend it

The idea moving forward is that not every criteria will necessarily be provided by the core package - it should be extensible by developers looking to implement something that might be quite specific to their application.  This should be mostly straightforward.  Due to the fact that the criteria that are made available come from a scan of all loaded assemblies, it should only be necessary to provide a dll with an implementation of **IPersonalisationGroupCriteria** and a unique `Alias` property, along with the definition editor angular view, controller and translation service - **definition.editor.html**, **definition.editor.controller.js** and **definition.definition.translator.js** respectively.

As well as the interface, there's a helper base class `PersonalisationGroupCriteriaBase` that you can inherit from that provides some useful methods for matching values and regular expressions.  This isn't required though for the criteria to be recognised and used.

The C# files can sit anywhere of course.  The client-side files should live in `App_Plugins/UmbracoPersonalisationGroups/GetResourceForCriteria/<criteriaAlias`.

As with other Umbraco packages, you'll also need to create a `package.manifest` file listing out the additional JavaScript files you need.  It should live in `App_Plugins/UmbracoPersonalisationGroups/` and look like this:

```
{
    javascript: [
        '~/App_Plugins/UmbracoPersonalisationGroups/GetResourceForCriteria/myAlias/definition.editor.controller.js',
        '~/App_Plugins/UmbracoPersonalisationGroups/GetResourceForCriteria/myAlias/definition.translator.js'
    ]    
}
```

## Working with caching

Caching - at least at the page level - and personalisation don't really play nicely together.  Such caching will normally be varied by the URL but with personalisation we are displaying different content to different users, so we don't want the cached version of a page customised to particular user being displayed to the next.

There are a couple of helper methods available within the package to help with this though.

Firstly there's an extension method associated with the Umbraco helper called `GetPersonalisationGroupsHashForVisitor()` that calculates a hash for the current visitor based on all the personalisation groups that apply to them.  In other words, if you've created three groups, it will determine whether the user matches each of those three groups and create a string based on the result.  It takes three parameters:

- Either an Id or an instance of the root node for the created personalisation groups
- An identifer for the user (most likely to be the ASP.Net session Id)
- A number of seconds to cache the calculation for

The last parameter is quite important - although not expensive, you likely don't want to calculate this value on every page request.  However it equally shouldn't be cached for too long as visitor's status in each personalisation group may change as they use the website.  For example a group targetting morning visitors would no longer match if the same visitor is still there in the afternoon.

With that method in available, it's possible to use it with output caching to ensure the cache varies by this set of matched personalisation groups, for example with a controller like this:

```
    public class TestPageController : RenderMvcController
    {
        [OutputCache(Duration = 600, VaryByParam = "*", VaryByCustom = "PersonalisationGroupsVisitorHash")]
        public override ActionResult Index(RenderModel model)
        {
            ...
        }

    }
```
	
And code in `global.asax.cs` as follows:

```
    public class Global : UmbracoApplication
    {
        private static readonly SessionStateSection SessionStateSection = (SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");

        public void Session_OnStart()
        {
            // Just set something to ensure a session is created
            Session[AppConstants.SessionKeys.PersonalisationGroupsEnsureSession] = 1;
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            if (custom.Equals("PersonalisationGroupsVisitorHash", StringComparison.OrdinalIgnoreCase))
            {
                var cookieName = SessionStateSection.CookieName;
                var sessionIdCookie = context.Request.Cookies[cookieName];
                if (sessionIdCookie != null)
                {
                    var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                    var hash = umbracoHelper.GetPersonalisationGroupsHashForVisitor(1093,   // Would normally get the node Id from config
                        sessionIdCookie.Value, 
                        20);
                    return hash;
                }
            }

            return base.GetVaryByCustomString(context, custom);
        }
    }
```

## Troubleshooting/known issues

### Personalisation group data type not loading

If you run into a problem with the data type failing to load when running with debug="false", this is because it's necessary to whitelist the domains in use.  See the [forum post here](https://our.umbraco.org/forum/umbraco-7/developing-umbraco-7-packages/64459-Single-file-property-editor-and-debug=false) along with links for discussion and resolution details.  In summary though:

- Open Config\ClientDependency.config
- Find the **bundleDomains** attribute
- Add a comma separated list of the domains you are using

*This has been resolved from version 0.1.11 for the criteria provided with the package, but there still looks to be a problem if you have created your own criteria using embedded resources as I've done so in the core package.  And then, even the bundleDomains workaround doesn't help.  So I believe it's necessary to avoid those and have the client-side files on disk as described in the section above.*

### Output cache being invalidated

In testing I've discovered that installing the package with default options will cause any output cache to be invalidated on every page request.  Clearly personalisation with output caching is likely tricky anyway (as by defintion, the same cached page may need to be presented differently to different users), so unlikely to be something being used.  If you do have a need for it though, it's necessary to disable any criteria that set cookies on each page request.  It's this action that invalidates the cache.

To do this you can exclude such critieria with this configuration option:

```
<add key="personalisationGroups.excludeCriteria" value="numberOfVisits,pagesViewed"/>
```

If you needed to personalise by these criteria - number of pages viewed and/or number of visits - it would be necessary to implement an alternate criteria that uses a different storage mechanism (such as a custom table or hooked into an analytics engine).

## Version history

- 0.1.0
    - Initial release
- 0.1.1 
     - Converted member type, group and profile field criteria to use drop-down list of available options for selection rather than free-text entry
- 0.1.2
     - Added comparison (greater than, less than etc.) options for session, cookie and member profile field
     - Fixed issue with saving of definition of day of week criteria
- 0.1.3
     - Added configuration option to specifically include or exclude certain criteria
- 0.1.4
    - Fixed some JavaScript errors in rendering translations for empty values
- 0.1.5
    - Amended time of day criteria internal JSON format to store times as strings rather than numbers (was causing some cross-browser issues)
- 0.1.6
    - No code changes - added missing dll to package
- 0.1.7
    - Added pages viewed criteria
- 0.1.8
    - Fixed issue where adding a new criteria but cancelling left an empty one that needed to be deleted
- 0.1.9
    - Added referrer criteria
- 0.1.10
    - Added number of site visits criteria
- 0.1.11
    - Fix for issue where embedded resources couldn't be loaded with debug="false" unless client dependency was configured with the domains in use - thanks to [James Jackson-South](https://github.com/JimBobSquarePants) for the pull request.
- 0.1.12
	- Removed a hard-coded test IP address from the CountryPersonalisationGroupCriteria implementation
- 0.1.13
	- Fixed a [reported issue with loading criteria from assemblies](https://our.umbraco.org/projects/website-utilities/personalisation-groups/issues-and-feedback/76753-anyone-tried-on-74)
- 0.1.14
	- Provided an optional boolean parameter to the `ShowToVisitor` extension method allowing the caller to indicate that if no groups have been configured the content should be shown or hidden.  Previously shown was assumed and this is still the default for this parameter.
- 0.1.15
	- Extended the country criteria to allow for matching visitors that are not in (as well as in) a given country.
	- This is a minor *breaking change* for anyone using this criteria as the JSON structure for the definition has been changed.  Any personalisation groups containing country criteria would need to have those criteria updated and resaved.
- 0.1.16
    - Added the output caching helper `GetPersonalisationGroupsHashForVisitor()`
- 0.2.0
    - Added querystring criteria (thanks to [Perplex](perplex.nl) for the code contribution
	- Creates base class `PersonalisationGroupCriteriaBase` providing common helper methods for all criteria
	- Added the "matches regular expression" from the querystring criteria to other appropriate ones
	- Allowed for configuration of cookie names used for number of visits and pages viewed tracking
	- Set all cookies to HttpOnly
	- Implemented "sticky" groups (thanks again to [Perplex](perplex.nl) for the suggestion
- 0.2.1
    - Ensured [IP used for geo-location does not have port number included](https://github.com/AndyButland/UmbracoPersonalisationGroups/pull/5)
- 0.2.2
    - Added further extension methods to allow passing in a list of groups to the method determining if a particular piece of content should be shown to a visitor.  Thanks to [Kevin Jump](https://github.com/KevinJump) for the [PR](https://github.com/AndyButland/UmbracoPersonalisationGroups/pull/6)	
- 0.2.3
	- Introduced `ScoreForVisitor()` extension method to allow personalised list ordering
- 0.2.4
	- Fixed bug with pages viewed criteria