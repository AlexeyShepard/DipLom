<?php

use Illuminate\Routing\Router;

Admin::routes();

Route::group([
    'prefix'        => config('admin.route.prefix'),
    'namespace'     => config('admin.route.namespace'),
    'middleware'    => config('admin.route.middleware'),
], function (Router $router) {

    $router->get('/', 'HomeController@index')->name('admin.home');
    $router->resource('users', UserController::class);
    $router->resource('peoples', PeopleController::class);
    $router->resource('notifications', NotificationController::class);
    $router->resource('events-logs', EventsLogsController::class);
    $router->resource('access-list', AccessListController::class);
    $router->resource('organizations', OrganizationController::class);

    $router->get('peoples/{id_people}/add-access', 'PeopleController@roleList');
    $router->get('peoples/{id_people}/add-access/{id_access}', 'PeopleController@addAccess');
    $router->get('peoples/{id_people}/delete-access/{id_accessInterim}', 'PeopleController@deleteAccess');

    $router->get('peoples/{id_people}/add-organization', 'PeopleController@organizationList');
    $router->get('peoples/{id_people}/add-organization/{id_organization}', 'PeopleController@addOrganization');
    $router->get('peoples/{id_people}/delete-organization/{id_peopleOrganization}', 'PeopleController@deleteOrganization');
    
    $router->get('tools', 'ToolController@index');
    $router->get('tools/generate-pincode', 'ToolController@GeneratePinCode');
   
});
