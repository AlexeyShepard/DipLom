<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Organization extends Model
{
    /**
    * Таблица, связанная с моделью.
    *
    * @var string
    */
    protected $table = 'Lom_Organizations';

  /**
    * @var bool
    */
    public $timestamps = false;
}
