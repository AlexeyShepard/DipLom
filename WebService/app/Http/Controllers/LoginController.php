<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Http\Controllers\BaseController as BaseController;
use App\People;

class LoginController extends BaseController
{
    public function login(Request $request)
    {
        $input = $request->all();

        $login = $input['Login'];
        $password = $input['Password'];

        $people = People::where([
            ['Login', '=', $login],
            ['Password', '=', $password]
        ])->first();

        if($people != null)
        {
            return $this->sendResponse($people->toArray(), 'Succesfull authorization');
        }
        else 
        {
            return $this->sendError('Authorization Fail');
        }
     }
}