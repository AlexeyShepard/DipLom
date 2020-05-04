<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Http\Controllers\BaseController as BaseController;
use App\PinCode;

class PinCodeController extends BaseController
{
    public function index()
    {
        $pinCode = PinCode::all();

        return $this->sendResponse($pinCode->toArray(), 'EventsHandlng retrieved successfully.');
    }
}
