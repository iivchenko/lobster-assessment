#addin Cake.Docker&version=0.10.0
#tool nuget:?package=NUnit.ConsoleRunner&version=3.10.0
var target = Argument("target", "Build");

Task("Build")
    .Does(() =>
{
	DockerComposeBuild("story.host");
});

Task("Run")
	.IsDependentOn("Build")
    .Does(() =>
{
	var settings = new DockerComposeUpSettings { DetachedMode = true };
	DockerComposeUp(settings, "story.host");
});

RunTarget(target);