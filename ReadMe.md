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
    - Session key presence/absence and value matching
    - Time of day
	- Umbraco member group
	- Umbraco member profile field	
	- Umbraco member type
- An extensible mechanism to allow other criteria to be created and loaded from other assemblies
- A property editor with associated angular controllers/views that provide the means of configuring personalisation groups based on the available criteria
- A single extension method on IPublishedContent named ShowToVisitor() that allows checking if the content should be available for the current site visitor

## Using the package

### Installation

Firstly install the package in the usual way.  

Once installed you'll find a few additional components:

 - **Personalisation Groups Folder** is a document type purely used to organise your personalisation groups.  By default you can create it at root and within it create further instances of itself.
 - **Personalisation Group** is a document type where the group itself is created.  You can place these inside the folders.  It comes with a single property called *Group definition* of a data type also provided with this package.
 - **Personalisation group definition** the data type provided to allow the definition of personalisation group, based on a new property editor provided in the dll installed with the package.
 - **Personalisation group picker** is an data type instance of the multi-node tree picker property editor, for the selection of groups for given piece of content
 
### Example usage

 - Within the "Content" section, create a root node of type **Personalisation Groups Folder**, called **Personalisation Groups**
 - Switch to "Developer" and find the **Personalisation group picker** data type.  Set the root node to be the node you just created.
 - Back to "Content", as a child of the node you just created, create a node of type **Personalisation Group** called, for example, *Weekday morning visitors*
     - Set the **Match** option to **All**
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
 - Finally you need to amend your template to make use of the personalisation group via new extension method that will be available on instances of **IPublishedContent**, named **ShowToVisitor()**, as described below.
 
## Templating
 
### Personalising repeated content
 
A typical example would be to personalise a list of repeated content to only show items that are appropriate for the current site visitor.  Here's how you might do that:
 
 	@foreach (var post in Model.Content.Children.Where(x => x.ShowToVisitor()))
	{
	    <h2>@post.Name</h2>
    }
		
With a little more work you can also personalise an individual page.  One way to do this would be to create sub-nodes of a page of a new type called e.g. "Page Variation".  This document type should contain all the fields common to the parent page that you might want to personalise - e.g. title, body text, image - and an instance of the "Personalisation group picker".  You could then implement some logic on the parent page template to pull back the first of the sub-nodes that match the current site visitor.  If one is found, you can display the content from that sub-node rather than what's defined for the page.  And if not, display the default content for the page.  Something like:

	@{
		var personalisedContent = Model.Content.Children.Where(x => x.ShowToVisitor()).FirstOrDefault();
		string title, bodyText;
		if (personalisedContent != null) 
		{
			title = personalisedContent.Name;
			bodyText = personalisedContent.GetPropertyValue<string>("bodyText);
		}
		else 
		{
			title = Model.Content.Name;
			bodyText = Model.Content.GetPropertyValue<string>("bodyText);	
		}
	}
	
	<h1>@title</h1>
	<p>@bodyText</p>

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

**PublishedContentExtensions** defines the extension method on **IPublishedContent** named **ShowToVisitor()**.  This implements the following logic:

- Checks for a group picker on the content, if there's not one then we return true (indicating to show to everyone)
- If found get the list of groups picked
- For each group picked, see if the definition provided matches the current site visitor.
    - If any one of them does, we return true (indicating to show the content)
	- If none of them do, we return false (indicating to hide the content)
	
## Notes on particular criteria

### Country

The country criteria uses the [free GeoLite2 IP to country database](http://dev.maxmind.com/geoip/geoip2/geolite2/) made available by Maxmind.com.  It'll look for it in /App_Data/GeoLite2-Country.mmdb or at the path specified in the following appSetting:

    <add key="personalisationGroups.groupPickerAlias" value="myCustomAlias"/> 	

## How to extend it

The idea moving forward is that not every criteria will necessarily be provided by the core package - it should be extensible by developers looking to implement something that might be quite specific to their application.  This should be mostly straightforward.  Due to the fact that the criteria that are made available come from a scan of all loaded assemblies, it should only be necessary to provide a dll with an implementation of **IPersonalisationGroupCriteria** along with the definition editor angular view, controller and translation service - **definition.editor.html**, **definition.editor.controller.js** and **definition.definition.translator.js** respectively - as embedded resources.

There's one gotcha though if you try to use embedded resources as used in the criteria classes in this package. Which is that it's not (I believe) possible to load an angular controller into Umbraco once the back-office application itself has bootstrapped.  The only way to do this is via a property editor, specifically it's **[PropertyEditorAsset]** attribute which is detailed [here](http://issues.umbraco.org/issue/U4-3712).

So in order to have the definition angular controller loaded, it's necessary to create a property editor to do this and have this defined within the extension dll, e.g.:

    /// <summary>
    /// This isn't a "real" property editor, it's rather a hack to inject the angular controller for this criteria definition builder.
    /// The PropertyEditorAsset attribute seems the only way currently to inject additional angular controllers before the application is bootstrapped.
    /// </summary>
    [PropertyEditor("personalisationGroupDefinitionDayOfWeek", "Visitor group definition (day of week - don't use)")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoPersonalisationGroups/ResourceForCriteria/dayOfWeek/definition.editor.controller.js")]
	[PropertyEditorAsset(ClientDependencyType.Javascript, "/App_Plugins/UmbracoPersonalisationGroups/ResourceForCriteria/dayOfWeek/definition.translator.js")]
    public class DummyDefinitionPropertyEditor : PropertyEditor
    {
    }

It doesn't need to be used (i.e. there's no need to create a data type from it).  And so the only downside if that you have this property editor available in the drop-down list for creating data types that you don't need.  

## Planned next steps

The following tasks are planned to continue development of this package:

- Update member group and type definition editors to use drop-down list rather than free text entry

## Troubleshooting/known issues

If you run into a problem with the data type failing to load when running with debug="false", this is because it's necessary to whitelist the domains in use.  See the [forum post here](https://our.umbraco.org/forum/umbraco-7/developing-umbraco-7-packages/64459-Single-file-property-editor-and-debug=false) along with links for discussion and resolution details.  In summary though:

- Open Config\ClientDependency.config
- Find the **bundleDomains** attribute
- Add a comma separated list of the domains you are using