<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class AccessList extends Model
{
    /**
    * Таблица, связанная с моделью.
    *
    * @var string
    */
    protected $table = 'Lom_AccessList';

  /**
    * @var bool
    */
    public $timestamps = false;
}
