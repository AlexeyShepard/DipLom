<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Http\Controllers\BaseController as BaseController;
use App\People;

class PeopleController extends BaseController
{
    public function index()
    {
        $people = People::all();

        return $this->sendResponse($people->toArray(), 'EventsHandlng retrieved successfully.');
    }
	
    public function show($id)
    {
        $people = People::find($id);

        return $this->sendResponse($people->toArray(), 'People retrieved successfully.');
    }

    public function store(Request $request)
    {
        $input = $request->all();

        try
        {
            $people = People::create($input);
            return $this->sendResponse($people->toArray(), 'Success');
        }
        catch(Exceception $e)
        {
            return $this->sendError($e, [], 500);
        }
    }
}
