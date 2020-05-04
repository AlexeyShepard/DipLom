<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class PinCode extends Model
{
    /**
    * Таблица, связанная с моделью.
    *
    * @var string
    */
    protected $table = 'Lom_PinCode';

  /**
    * @var bool
    */
    public $timestamps = false;
}
