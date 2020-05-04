<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Http\Controllers\BaseController as BaseController;
use App\EventsHandling;
use Validator;

class EventsHandlingController extends BaseController
{
    public function index()
    {
        $eventsHandling = EventsHandling::all();

        return $this->sendResponse($eventsHandling->toArray(), 'EventsHandlng retrieved successfully.');
    }

    public function store(Request $request)
    {
        $input = $request->all();

        $eventsHandling = EventsHandling::create($input);
        return $this->sendResponse($eventsHandling->toArray(), 'EventsHandling created successfully.');
    }

}
